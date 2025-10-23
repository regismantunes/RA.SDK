using RA.Console.DependencyInjection.Args;

namespace RA.Console.DependencyInjection.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CommandAsyncWithArgsBuilderAttribute<TArgsBuilder>(params string[] commands) :
        CommandAsyncAttribute(commands),
        ICommandWithArgsBuilderAttribute<TArgsBuilder> where TArgsBuilder : IArgsBuilder
    { }
}