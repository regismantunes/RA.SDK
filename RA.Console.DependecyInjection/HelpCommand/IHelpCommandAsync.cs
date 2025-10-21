namespace RA.Console.DependecyInjection.HelpCommand
{
    public interface IHelpCommandAsync
    {
        Task<int> ExecuteAsync(IEnumerable<CommandInfo> commands, CancellationToken cancellationToken = default);
    }
}