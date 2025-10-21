namespace RA.Console.DependecyInjection.Args
{
    public interface IArgsBuilderAsync
    {
        Task<IDictionary<string, object>> BuildAsync(string[] args, CancellationToken cancellationToken = default);
    }
}
