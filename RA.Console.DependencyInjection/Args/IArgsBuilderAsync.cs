namespace RA.Console.DependencyInjection.Args
{
    public interface IArgsBuilderAsync
    {
        Task<IDictionary<string, object>> BuildAsync(string[] args, CancellationToken cancellationToken = default);
    }
}
