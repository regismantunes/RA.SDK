namespace RA.Console.DependencyInjection.HelpCommand
{
    /// <summary>
    /// Represents an asynchronous help command capable of rendering help content.
    /// </summary>
    public interface IHelpCommandAsync
    {
        /// <summary>
        /// Executes the help command asynchronously using the provided command metadata.
        /// </summary>
        /// <param name="commands">The collection of commands to display.</param>
        /// <param name="cancellationToken">A token to observe while the operation is in progress.</param>
        /// <returns>A task that resolves to the exit code.</returns>
        Task<int> ExecuteAsync(IEnumerable<CommandInfo> commands, CancellationToken cancellationToken = default);
    }
}