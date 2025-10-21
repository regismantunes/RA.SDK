
namespace RA.Console.DependecyInjection
{
    public interface IConsoleApp
    {
        IServiceProvider Services { get; }

        Task<int> RunAsync(CancellationToken cancellationToken = default);
    }
}