using RA.Console.DependecyInjection.Attributes;

namespace RA.Console.DependecyInjection
{
    public record CommandInfo(
        string[] Commands,
        string Description,
        string Example,
        string? Group,
        int Order)
    {
        public static CommandInfo GetInfo(CommandAttribute commandAttribute)
        {
            return new CommandInfo(
                commandAttribute.Commands,
                commandAttribute.Description,
                commandAttribute.Example,
                commandAttribute.Group,
                commandAttribute.Order);
        }
    }
}
