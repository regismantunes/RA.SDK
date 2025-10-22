namespace RA.Console.DependecyInjection.Middleware
{
    public interface ICommandMiddleware
    {
        Task InvokeAsync(CommandContext context, Func<CommandContext, Task> next);
    }
}
