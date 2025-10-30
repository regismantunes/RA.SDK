namespace RA.Console.DependencyInjection
{
    /// <summary>
    /// Represents a console application capable of running commands with dependency injection support.
    /// </summary>
    public interface IConsoleApp
    {
        /// <summary>
        /// The application's service provider.
        /// </summary>
        IServiceProvider Services { get; }

        /// <summary>
        /// Runs the console application.
        /// </summary>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A task that resolves to the application exit code.</returns>
        Task<int> RunAsync(CancellationToken cancellationToken = default);
    }
}