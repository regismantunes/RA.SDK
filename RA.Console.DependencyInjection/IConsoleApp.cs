
namespace RA.Console.DependencyInjection
{
    public interface IConsoleApp
    {
        IServiceProvider Services { get; }

        Task<int> RunAsync(CancellationToken cancellationToken = default);
    }
}