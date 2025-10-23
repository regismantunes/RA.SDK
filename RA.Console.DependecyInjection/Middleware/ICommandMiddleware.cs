namespace RA.Console.DependecyInjection.Middleware
{
    public interface ICommandMiddleware
    {
        Task<int> InvokeAsync(CommandContext context, Func<CommandContext, Task<int>> next);
    }
}
