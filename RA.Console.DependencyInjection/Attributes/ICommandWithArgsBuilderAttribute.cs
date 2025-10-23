using RA.Console.DependencyInjection.Args;

namespace RA.Console.DependencyInjection.Attributes
{
    public interface ICommandWithArgsBuilderAttribute<TArgsBuilder> where TArgsBuilder : IArgsBuilder
    { }
}
