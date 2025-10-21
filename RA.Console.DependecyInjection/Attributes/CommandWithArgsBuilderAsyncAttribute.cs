using RA.Console.DependecyInjection.Args;

namespace RA.Console.DependecyInjection.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CommandWithArgsBuilderAsyncAttribute<TArgsBuilderAsync>(params string[] commands) :
        CommandAttribute(commands),
        ICommandWithArgsBuilderAsyncAttribute<TArgsBuilderAsync> where TArgsBuilderAsync : IArgsBuilderAsync
    { }
}