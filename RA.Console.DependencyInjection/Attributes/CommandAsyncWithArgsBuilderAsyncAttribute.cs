using RA.Console.DependencyInjection.Args;

namespace RA.Console.DependencyInjection.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CommandAsyncWithArgsBuilderAsyncAttribute<TArgsBuilderAsync>(params string[] commands) :
        CommandAsyncAttribute(commands),
        ICommandWithArgsBuilderAsyncAttribute<TArgsBuilderAsync> where TArgsBuilderAsync : IArgsBuilderAsync
    { }
}