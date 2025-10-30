namespace RA.Console.DependencyInjection.Middleware
{
    /// <summary>
    /// Represents a middleware in the command execution pipeline.
    /// </summary>
    public interface ICommandMiddleware
    {
        /// <summary>
        /// Invokes the middleware with the provided context.
        /// </summary>
        /// <param name="context">The command execution context.</param>
        /// <param name="next">The delegate to invoke the next middleware.</param>
        /// <returns>A task that resolves to the command's exit code.</returns>
        Task<int> InvokeAsync(CommandContext context, Func<CommandContext, Task<int>> next);
    }
}
