using Microsoft.Extensions.DependencyInjection;
using RA.Console.DependecyInjection.Args;
using RA.Console.DependecyInjection.Attributes;
using RA.Console.DependecyInjection.HelpCommand;
using System.Reflection;

namespace RA.Console.DependecyInjection
{
    public class ConsoleApp : IConsoleApp
    {
        private readonly ConsoleAppStartInfo _startInfo;

        public IServiceProvider Services { get; }

        internal ConsoleApp(ConsoleAppStartInfo startInfo)
        {
            _startInfo = startInfo ?? throw new ArgumentNullException(nameof(startInfo));
            Services = startInfo.ServiceProvider;
        }

        public async Task<int> RunAsync(CancellationToken cancellationToken = default)
        {
            Current = this;
            if (_startInfo.IsHelpCommand)
            {
                if (_startInfo.HelpCommandInitializationInfo == null)
                    throw new InvalidOperationException("Help command initialization info is not defined.");

                var helpCommandInitializationInfo = _startInfo.HelpCommandInitializationInfo.Value;
                if (helpCommandInitializationInfo.HelpCommand == null)
                    throw new InvalidOperationException("Help command is not defined.");

                var helpCommandObject = Services.GetRequiredService(helpCommandInitializationInfo.HelpCommand);
                if (helpCommandObject is IHelpCommand helpCommand)
                    return helpCommand.Execute(helpCommandInitializationInfo.HelpCommandParameter);
                else if (helpCommandObject is IHelpCommandAsync helpCommandAsync)
                    return await helpCommandAsync.ExecuteAsync(helpCommandInitializationInfo.HelpCommandParameter, cancellationToken);
                else
                    throw new InvalidOperationException("Help command does not implement a valid interface.");
            }

            if (_startInfo.Command is null)
                throw new InvalidOperationException("Command is not defined.");

            (var commandMethod, var commandAttribute) = _startInfo.Commands.TryGetValue(_startInfo.Command, out var methodPair)
                ? methodPair
                : throw new InvalidOperationException($"Command '{_startInfo.Command}' not found.");

            var parameters = await BuildParametersAsync(commandMethod, commandAttribute, _startInfo.Args, cancellationToken);
            var commandHandler = Services.GetRequiredService(commandMethod.DeclaringType!);

            if (typeof(CommandAsyncAttribute).IsAssignableFrom(commandAttribute.GetType()))
                return await (Task<int>)commandMethod.Invoke(commandHandler, parameters);
            else if (typeof(CommandAttribute).IsAssignableFrom(commandAttribute.GetType()))
                return (int)commandMethod.Invoke(commandHandler, parameters);
            else
                throw new InvalidOperationException($"Command attribute for command '{_startInfo.Command}' is of an unknown type.");
        }

        private async Task<object?[]> BuildParametersAsync(MethodInfo methodInfo, CommandAttribute commandAttribute, string[] args, CancellationToken cancellationToken)
        {
            var methodParameters = methodInfo.GetParameters();
            if (methodParameters.Length == 0)
                return [];

            var methodArgs = await BuildArgsAsync(methodInfo, commandAttribute, args, cancellationToken);

            var buildedParameters = new object?[methodParameters.Length];
            for (var i = 0; i < methodParameters.Length; i++)
            {
                var parameter = methodParameters[i];
                if (parameter.ParameterType == typeof(CancellationToken))
                {
                    buildedParameters[i] = cancellationToken;
                    continue;
                }

                var parameterValue = GetParameterValue(parameter, methodArgs, methodInfo.Name);

                ValidateParameterValue(parameterValue, parameter, methodInfo.Name);

                buildedParameters[i] = parameterValue;
            }

            return buildedParameters;
        }

        private async Task<IDictionary<string, object>> BuildArgsAsync(MethodInfo methodInfo, CommandAttribute commandAttribute, string[] args, CancellationToken cancellationToken)
        {
            var argsBuilderObject = InitializaArgsBuilder(commandAttribute, methodInfo.Name);
            IDictionary<string, object> methodArgs;
            if (argsBuilderObject is null)
                methodArgs = DefaultArgsBuilder.Build(methodInfo, args);
            else if (argsBuilderObject is IArgsBuilder argsBuilder)
                methodArgs = argsBuilder.Build(args);
            else if (argsBuilderObject is IArgsBuilderAsync argsBuilderAsync)
                methodArgs = await argsBuilderAsync.BuildAsync(args, cancellationToken);
            else
                throw new InvalidOperationException($"ArgsBuilder for command '{methodInfo.Name}' must implement IArgsBuilder or IArgsBuilderAsync.");
            return methodArgs;
        }

        private object? InitializaArgsBuilder(CommandAttribute commandAttribute, string methodName)
        {
            object? argsBuilderObject = null;
            var commandAttributeType = commandAttribute.GetType();
            if (commandAttributeType.IsGenericType)
            {
                var argsBuilderType = commandAttributeType
                    .GetGenericArguments()
                    .FirstOrDefault()
                    ?? throw new InvalidOperationException($"ArgsBuilder type not found in CommandAttribute for command '{methodName}'.");
                argsBuilderObject = Services.GetRequiredService(argsBuilderType);
            }
            return argsBuilderObject;
        }

        private static object? GetParameterValue(ParameterInfo parameter, IDictionary<string, object> methodArgs, string methodName)
        {
            var parameterName = parameter.Name;
            var parameterNameIsCaseSencitive = false;
            var parameterAttribute = parameter.GetCustomAttribute<ParameterAttribute>();
            if (parameterAttribute != null)
            {
                parameterName = parameterAttribute.Name;
                parameterNameIsCaseSencitive = parameterAttribute.IsCaseSensitive;
            }

            var methodArgsKey = methodArgs.Keys.FirstOrDefault(k => parameterNameIsCaseSencitive ?
                                                                    k == parameterName :
                                                                    k.Equals(parameterName, StringComparison.InvariantCultureIgnoreCase));
            var parameterValue = methodArgsKey == null ? null : methodArgs[methodArgsKey];
            if (parameterValue is null)
            {
                if (parameter.HasDefaultValue)
                    parameterValue = parameter.DefaultValue;
                else if (parameter.IsOptional)
                    parameterValue = null;
                else
                    throw new ArgumentException(
                        $"Missing argument '{parameter.Name}' for method '{methodName}'.");
            }

            //return TryToConvertParameterValue(parameter, parameterValue, methodName);
            return parameterValue;
        }

        private static void ValidateParameterValue(object? parameterValue, ParameterInfo parameter, string methodName)
        {
            if (parameterValue is null &&
                parameter.ParameterType.IsValueType &&
                Nullable.GetUnderlyingType(parameter.ParameterType) is null)
                throw new ArgumentException(
                    $"Argument '{parameter.Name}' for method '{methodName}' cannot be null.");

            if (parameterValue is not null &&
                !parameter.ParameterType.IsInstanceOfType(parameterValue))
                throw new ArgumentException(
                    $"Argument '{parameter.Name}' for method '{methodName}' is of incorrect type. Expected '{parameter.ParameterType.FullName}', got '{parameterValue.GetType().FullName}'.");
        }

        /*private static object? TryToConvertParameterValue(ParameterInfo parameter, object? parameterValue, string methodName)
        {
            var parameterAttributeGeneric = parameter.GetCustomAttribute(typeof(ParameterAttribute<,>));
            if (parameterAttributeGeneric != null)
            {
                var parameterAttributeGenericInfo = parameterAttributeGeneric.GetType().GetTypeInfo();
                var converterProperty = parameterAttributeGenericInfo.GetProperty("Converter")
                    ?? throw new InvalidOperationException(
                        $"Converter property not found in ParameterAttribute for parameter '{parameter.Name}' in method '{methodName}'.");
                var converter = converterProperty.GetValue(parameterAttributeGeneric)
                    ?? throw new InvalidOperationException(
                        $"Converter is null in ParameterAttribute for parameter '{parameter.Name}' in method '{methodName}'.");
                var converterType = converter.GetType();
                var converterMethod = converterType.GetMethod("Invoke");
                if (converterMethod == null)
                    throw new InvalidOperationException(
                        $"Invoke method not found in converter for parameter '{parameter.Name}' in method '{methodName}'.");
                try
                {
                    parameterValue = converterMethod.Invoke(converter, [parameterValue]);
                }
                catch (TargetInvocationException ex)
                {
                    throw new ArgumentException(
                        $"Error converting argument '{parameter.Name}' for method '{methodName}': {ex.InnerException?.Message}", ex.InnerException);
                }
            }
            return parameterValue;
        }*/

        private static readonly Lock _lock = new();
        public static ConsoleApp? Current
        {
            get;
            private set
            {
                lock (_lock)
                {
                    if (field is not null)
                        throw new InvalidOperationException("A ConsoleApp instance is already running.");

                    field = value;
                }
            }
        }
    }
}
