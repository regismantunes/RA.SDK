using RA.Console.DependencyInjection.Args;

namespace RA.Console.DependencyInjection.Attributes
{
    /// <summary>
    /// Marker interface for command attributes that specify a synchronous arguments builder.
    /// </summary>
    /// <typeparam name="TArgsBuilder">The arguments builder type implementing <see cref="Args.IArgsBuilder"/>.</typeparam>
    public interface ICommandWithArgsBuilderAttribute<TArgsBuilder> where TArgsBuilder : IArgsBuilder
    { }
}
