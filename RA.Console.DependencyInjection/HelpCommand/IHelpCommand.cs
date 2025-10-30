namespace RA.Console.DependencyInjection.HelpCommand
{
    /// <summary>
    /// Represents a synchronous help command capable of rendering help content.
    /// </summary>
    public interface IHelpCommand
    {
        /// <summary>
        /// Executes the help command using the provided command metadata.
        /// </summary>
        /// <param name="commands">The collection of commands to display.</param>
        /// <returns>The exit code.</returns>
        int Execute(IEnumerable<CommandInfo> commands);
    }
}