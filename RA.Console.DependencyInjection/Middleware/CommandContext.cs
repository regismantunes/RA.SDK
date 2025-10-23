using RA.Console.DependencyInjection.Attributes;
using System.Reflection;

namespace RA.Console.DependencyInjection.Middleware
{
    public record CommandContext
    {
        public string? Command { get; init; } = string.Empty;
        public bool IsHelpCommand { get; init; } = false;
        public string[] Args { get; init; } = [];
        public CommandAttribute? CommandAttribute { get; init; }
        public required Type CommandClass { get; init; }
        public MethodInfo? CommandMethod { get; init; }
        public Type? ArgsBuilder { get; init; }
        public CancellationToken CancellationToken { get; init; } = CancellationToken.None;
    }
}