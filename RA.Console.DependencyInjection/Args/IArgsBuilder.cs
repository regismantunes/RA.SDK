namespace RA.Console.DependencyInjection.Args
{
    /// <summary>
    /// Builds a dictionary of argument values for a command method from raw command-line args.
    /// </summary>
    public interface IArgsBuilder
    {
        /// <summary>
        /// Parses raw arguments into a name/value dictionary for parameter binding.
        /// </summary>
        /// <param name="args">The raw command-line arguments.</param>
        /// <returns>A dictionary mapping parameter names to values.</returns>
        IDictionary<string, object> Build(string[] args);
    }
}
