namespace RA.Console.DependecyInjection.Args
{
    public interface IArgsBuilder
    {
        IDictionary<string, object> Build(string[] args);
    }
}
