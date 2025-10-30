using RA.Console.DependencyInjection.Args;

namespace RA.Console.DependencyInjection.Attributes
{
    /// <summary>
    /// Marker interface for command attributes that specify an asynchronous arguments builder.
    /// </summary>
    /// <typeparam name="TArgsBuilderAsync">The async arguments builder type implementing <see cref="Args.IArgsBuilderAsync"/>.</typeparam>
    public interface ICommandWithArgsBuilderAsyncAttribute<TArgsBuilderAsync> where TArgsBuilderAsync : IArgsBuilderAsync
    { }
}
