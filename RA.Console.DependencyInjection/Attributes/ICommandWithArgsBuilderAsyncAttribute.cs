using RA.Console.DependencyInjection.Args;

namespace RA.Console.DependencyInjection.Attributes
{
    public interface ICommandWithArgsBuilderAsyncAttribute<TArgsBuilderAsync> where TArgsBuilderAsync : IArgsBuilderAsync
    { }
}
