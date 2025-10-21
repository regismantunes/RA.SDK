using Microsoft.Extensions.DependencyInjection;
using RA.Console.DependecyInjection.Args;
using RA.Console.DependecyInjection.Attributes;
using RA.Console.DependecyInjection.HelpCommand;
using System.Reflection;

namespace RA.Console.DependecyInjection
{
    public class ConsoleAppBuilder(string[]? args)
    {
        private readonly string[] _args = args ?? [];
        public IServiceCollection Services { get; } = new ServiceCollection();
        
        #region Assemblies
        private IList<Assembly> Assemblies { get; } = [];
        
        public ConsoleAppBuilder AddAssembly(Assembly assembly)
        {
            ArgumentNullException.ThrowIfNull(assembly, nameof(assembly));
            Assemblies.Add(assembly);
            return this;
        }

        public ConsoleAppBuilder AddAssembly<T>()
            => AddAssembly(typeof(T).Assembly);

        public ConsoleAppBuilder AddAssemblies(params IEnumerable<Assembly> assemblies)
        {
            ArgumentNullException.ThrowIfNull(assemblies, nameof(assemblies));
            foreach (var assembly in assemblies)
                AddAssembly(assembly);
            return this;
        }
        #endregion

        #region Initialization Options

        private bool OptimizedInitialization { get; set; }

        public ConsoleAppBuilder UseOptimizedInitialization()
        {
            OptimizedInitialization = true;
            return this;
        }

        #endregion

        #region Help
        private IEnumerable<string>? HelpCommands { get; set; }
        private bool EmptyArgsForHelp { get; set; }
        private Type? HelpCommand { get; set; }
        
        public ConsoleAppBuilder SetHelpCommands(params IEnumerable<string> helpCommands)
        {
            HelpCommands = helpCommands;
            return this;
        }

        public ConsoleAppBuilder UseDefaultHelpCommands()
        {
            HelpCommands = ["-h", "--help", "/?", "help"];
            return this;
        }

        public ConsoleAppBuilder UseEmptyArgsForHelp()
        {
            EmptyArgsForHelp = true;
            return this;
        }

        private bool IsHelpCommand()
        {
            if (HelpCommands is null ||
                _args.Length == 0)
                return EmptyArgsForHelp;
            return HelpCommands.Contains(_args[0], StringComparer.OrdinalIgnoreCase);
        }

        public ConsoleAppBuilder SetHelpCommand<T>() where T : IHelpCommand
        {
            HelpCommand = typeof(T);
            return this;
        }

        public ConsoleAppBuilder SetHelpCommandAsync<T>() where T : IHelpCommandAsync
        {
            HelpCommand = typeof(T);
            return this;
        }

        public ConsoleAppBuilder UseDefaultHelpCommand()
        {
            HelpCommand = typeof(DefaultHelpCommand);
            return this;
        }

        public ConsoleAppBuilder UseDefaultHelpResources()
            => UseDefaultHelpCommands()
                .UseEmptyArgsForHelp()
                .UseDefaultHelpCommand();

        #endregion

        #region Build
        public IConsoleApp Build()
        {
            var requestedCommand = _args.Length > 0 ? _args[0] : null;
            var initializationCommand = OptimizedInitialization ? requestedCommand : null;
            var isHelpCommand = IsHelpCommand();
            var loadHelpCommandInitializationInfo = isHelpCommand || !OptimizedInitialization;
            var loadHelpCommand = HelpCommand is null && loadHelpCommandInitializationInfo;
            var loadCommonCommands = !isHelpCommand || !OptimizedInitialization;

            LoadServicesFromRegisteredAssemblies(
                loadHelpCommandInitializationInfo,
                loadHelpCommand,
                loadCommonCommands,
                out var helpCommandInitializationInfo,
                out var commandMethods,
                initializationCommand);

            if (loadHelpCommandInitializationInfo &&
                helpCommandInitializationInfo.HelpCommand is null)
                throw new NoCommandsDefinedException("HelpCommand class not defined in the registered assemblies.");

            if (commandMethods.Count == 0 && !isHelpCommand)
                throw new NoCommandsDefinedException(initializationCommand is null ?
                    "No commands defined in the registered assemblies." :
                    $"No commands defined for '{initializationCommand}' in the registered assemblies.");

            if (loadHelpCommandInitializationInfo && !loadHelpCommand)
                Services.AddSingleton(HelpCommand!);

            var dictionaryCommands = new Dictionary<string, (MethodInfo, CommandAttribute)>(StringComparer.OrdinalIgnoreCase);
            foreach (var (methodInfo, commandAttribute) in commandMethods)
            {
                commandAttribute.Validate(methodInfo);
                foreach (var command in commandAttribute.Commands)
                {
                    if (dictionaryCommands.ContainsKey(command))
                        throw new MultipleCommandsDefinedException($"Multiple commands found for '{command}'.");
                    dictionaryCommands.Add(command, (methodInfo, commandAttribute));
                }
            }

            var consoleAppStartInfo = new ConsoleAppStartInfo()
            {
                Command = requestedCommand,
                Args = _args,
                HelpCommandInitializationInfo = helpCommandInitializationInfo,
                IsHelpCommand = isHelpCommand,
                Commands = dictionaryCommands,
                ServiceProvider = Services.BuildServiceProvider()
            };

            return new ConsoleApp(consoleAppStartInfo);
        }

        private void LoadServicesFromRegisteredAssemblies(
            bool loadHelpCommandInitializationInfo,
            bool loadHelpCommand,
            bool loadCommonCommands,
            out HelpCommandInitializationInfo helpCommandInitializationInfo,
            out List<(MethodInfo, CommandAttribute)> commandMethods,
            string? command = null)
        {
            commandMethods = [];
            helpCommandInitializationInfo = new HelpCommandInitializationInfo() { HelpCommand = HelpCommand, HelpCommandParameter = [] };
            foreach (var assembly in Assemblies)
            {
                LoadServicesFromAssembly(
                    assembly,
                    loadHelpCommandInitializationInfo,
                    loadHelpCommand,
                    loadCommonCommands,
                    ref helpCommandInitializationInfo,
                    out var assemblyCommandMethods,
                    command);
                
                if (assemblyCommandMethods is null ||
                    assemblyCommandMethods.Count == 0)
                    continue;

                if (command is not null &&
                    (commandMethods.Count != 0 || assemblyCommandMethods.Count > 1))
                    throw new MultipleCommandsDefinedException($"Multiple commands found for '{command}' in different assemblies.");
                
                commandMethods.AddRange(assemblyCommandMethods);
            }
        }

        private void LoadServicesFromAssembly(
            Assembly assembly,
            bool loadHelpCommandInitializationInfo,
            bool loadHelpCommand,
            bool loadCommonCommands,
            ref HelpCommandInitializationInfo helpCommandInitializationInfo,
            out List<(MethodInfo, CommandAttribute)> commandMethods,
            string? command = null)
        {
            commandMethods = [];
            var assemblyClasses = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract);
            foreach (var assemblyClass in assemblyClasses)
            {
                var registerClassAsService = false;
                if (loadHelpCommand)
                {
                    if (typeof(IHelpCommand).IsAssignableFrom(assemblyClass) ||
                        typeof(IHelpCommandAsync).IsAssignableFrom(assemblyClass))
                    {
                        if (helpCommandInitializationInfo.HelpCommand is not null)
                            throw new MultipleCommandsDefinedException($"Multiple HelpCommand classes found.");

                        helpCommandInitializationInfo.HelpCommand = assemblyClass;
                        registerClassAsService = true;
                    }
                }

                if (loadCommonCommands ||
                    loadHelpCommandInitializationInfo)
                {
                    foreach (var method in assemblyClass.GetMethods())
                    {
                        var commandsAttributes = method.GetCustomAttributes(inherit: true)
                            .OfType<CommandAttribute>()
                            .Where(ca => loadHelpCommandInitializationInfo ||
                                         command is null ||
                                         ca.Commands.Contains(command, StringComparer.OrdinalIgnoreCase));
                        if (!commandsAttributes.Any())
                            continue;
                        if (command is not null)
                        {
                            if (commandMethods.Count > 0)
                                throw new MultipleCommandsDefinedException(
                                    $"Multiple commands found for '{command}' in assembly '{assembly.FullName}'.");
                            if (commandsAttributes.Count() > 1)
                                throw new MultipleCommandsDefinedException(
                                    $"Multiple commands found for '{command}' in method '{method.Name}' of class '{assemblyClass.FullName}'.");
                        }

                        if (loadHelpCommandInitializationInfo)
                            helpCommandInitializationInfo.HelpCommandParameter
                                .AddRange(commandsAttributes
                                            .Where(ca => !ca.Hide)
                                            .Select(ca => new CommandInfo(ca.Commands, ca.Description, ca.Example, ca.Group, ca.Order))
                                    );

                        if (loadCommonCommands)
                        {
                            registerClassAsService = true;
                            commandMethods.AddRange(commandsAttributes
                                .Where(ca => command is null ||
                                             ca.Commands.Contains(command, StringComparer.OrdinalIgnoreCase))
                                .Select(ca => (method, ca))
                            );
                            if (command is not null)
                            {
                                var commandAttributeType = commandsAttributes
                                    .Single()
                                    .GetType();
                                if (commandAttributeType.IsGenericType)
                                {
                                    var argsBuilderType = commandAttributeType
                                        .GetGenericArguments()
                                        .FirstOrDefault()
                                        ?? throw new InvalidOperationException($"ArgsBuilder type not found in CommandAttribute for command '{command}'.");
                                    Services.AddSingleton(argsBuilderType);
                                }
                            }
                        }
                    }
                }

                if (loadCommonCommands && command is null) //Load all args builders only if not searching for a specific command
                {
                    if (typeof(IArgsBuilder).IsAssignableFrom(assemblyClass) ||
                        typeof(IArgsBuilderAsync).IsAssignableFrom(assemblyClass))
                        registerClassAsService = true;
                }

                if (registerClassAsService)
                {
                    Services.AddSingleton(assemblyClass);
                }
            }
        }
        #endregion
    }
}