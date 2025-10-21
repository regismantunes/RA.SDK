namespace RA.Console.DependecyInjection.HelpCommand
{
    public interface IHelpCommand
    {
        int Execute(IEnumerable<CommandInfo> commands);
    }
}