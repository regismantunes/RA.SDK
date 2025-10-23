namespace RA.Console.DependencyInjection.Middleware
{
    public interface ICommandMiddleware
    {
        Task<int> InvokeAsync(CommandContext context, Func<CommandContext, Task<int>> next);
    }
}
