using RA.Console.DependecyInjection.Args;

namespace RA.Console.DependecyInjection.Attributes
{
    public interface ICommandWithArgsBuilderAsyncAttribute<TArgsBuilderAsync> where TArgsBuilderAsync : IArgsBuilderAsync
    { }
}
