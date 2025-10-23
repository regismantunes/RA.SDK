namespace RA.Console.DependencyInjection.HelpCommand
{
    public interface IHelpCommand
    {
        int Execute(IEnumerable<CommandInfo> commands);
    }
}