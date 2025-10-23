namespace RA.Console.DependencyInjection.HelpCommand
{
    public interface IHelpCommandAsync
    {
        Task<int> ExecuteAsync(IEnumerable<CommandInfo> commands, CancellationToken cancellationToken = default);
    }
}