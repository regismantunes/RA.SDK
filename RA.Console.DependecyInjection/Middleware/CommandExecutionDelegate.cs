namespace RA.Console.DependecyInjection.Middleware
{
    public delegate Task<int> CommandExecutionDelegate(CommandContext context);
}
