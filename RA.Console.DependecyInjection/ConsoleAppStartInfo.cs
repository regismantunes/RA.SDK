using RA.Console.DependecyInjection.Attributes;
using RA.Console.DependecyInjection.HelpCommand;
using System.Reflection;

namespace RA.Console.DependecyInjection
{
    internal record ConsoleAppStartInfo()
    {
        public required string? Command { get; init; }
        public required string[] Args { get; init; }
        public required HelpCommandInitializationInfo? HelpCommandInitializationInfo { get; init; }
        public required bool IsHelpCommand { get; init; }
        public required IDictionary<string, (MethodInfo, CommandAttribute)> Commands { get; init; }
        public required IServiceProvider ServiceProvider { get; init; }
    }
}
