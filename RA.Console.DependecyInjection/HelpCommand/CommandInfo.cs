namespace RA.Console.DependecyInjection.HelpCommand
{
    public record CommandInfo(
        string[] Commands,
        string Description,
        string Example,
        string? Group,
        int Order);
}
