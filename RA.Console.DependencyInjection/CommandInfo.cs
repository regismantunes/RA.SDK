using RA.Console.DependencyInjection.Attributes;

namespace RA.Console.DependencyInjection
{
    /// <summary>
    /// Represents metadata about a console command used for help output.
    /// </summary>
    /// <param name="Commands">The command aliases.</param>
    /// <param name="Description">The description of the command.</param>
    /// <param name="Example">An example usage string.</param>
    /// <param name="Group">The group name for the command.</param>
    /// <param name="Order">The sorting order within its group.</param>
    public record CommandInfo(
        string[] Commands,
        string Description,
        string Example,
        string? Group,
        int Order)
    {
        /// <summary>
        /// Creates a <see cref="CommandInfo"/> instance from a <see cref="Attributes.CommandAttribute"/>.
        /// </summary>
        /// <param name="commandAttribute">The command attribute to convert.</param>
        /// <returns>A populated <see cref="CommandInfo"/>.</returns>
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
