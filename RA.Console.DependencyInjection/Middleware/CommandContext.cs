using RA.Console.DependencyInjection.Attributes;
using System.Reflection;

namespace RA.Console.DependencyInjection.Middleware
{
    /// <summary>
    /// Represents the context for a command execution, passed through the middleware pipeline.
    /// </summary>
    public record CommandContext
    {
        /// <summary>
        /// The command name being executed.
        /// </summary>
        public string? Command { get; init; } = string.Empty;
        /// <summary>
        /// Indicates whether the current execution is for the help command.
        /// </summary>
        public bool IsHelpCommand { get; init; } = false;
        /// <summary>
        /// The raw command-line arguments.
        /// </summary>
        public string[] Args { get; init; } = [];
        /// <summary>
        /// The command attribute associated with the command method.
        /// </summary>
        public CommandAttribute? CommandAttribute { get; init; }
        /// <summary>
        /// The type of the class that contains the command method or help handler.
        /// </summary>
        public required Type CommandClass { get; init; }
        /// <summary>
        /// The method to invoke for the command, when applicable.
        /// </summary>
        public MethodInfo? CommandMethod { get; init; }
        /// <summary>
        /// The type of the arguments builder to use, when applicable.
        /// </summary>
        public Type? ArgsBuilder { get; init; }
        /// <summary>
        /// A token to observe during command execution.
        /// </summary>
        public CancellationToken CancellationToken { get; init; } = CancellationToken.None;
    }
}