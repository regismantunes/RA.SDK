namespace RA.Console.DependencyInjection.Args
{
    /// <summary>
    /// Asynchronously builds a dictionary of argument values for a command method from raw command-line args.
    /// </summary>
    public interface IArgsBuilderAsync
    {
        /// <summary>
        /// Parses raw arguments into a name/value dictionary for parameter binding.
        /// </summary>
        /// <param name="args">The raw command-line arguments.</param>
        /// <param name="cancellationToken">A token to observe while the operation is in progress.</param>
        /// <returns>A task that resolves to a dictionary mapping parameter names to values.</returns>
        Task<IDictionary<string, object>> BuildAsync(string[] args, CancellationToken cancellationToken = default);
    }
}
