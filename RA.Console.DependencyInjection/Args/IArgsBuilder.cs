namespace RA.Console.DependencyInjection.Args
{
    public interface IArgsBuilder
    {
        IDictionary<string, object> Build(string[] args);
    }
}
