using RA.Console.DependencyInjection.Args;

namespace RA.Console.DependencyInjection.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CommandWithArgsBuilderAsyncAttribute<TArgsBuilderAsync>(params string[] commands) :
        CommandAttribute(commands),
        ICommandWithArgsBuilderAsyncAttribute<TArgsBuilderAsync> where TArgsBuilderAsync : IArgsBuilderAsync
    { }
}