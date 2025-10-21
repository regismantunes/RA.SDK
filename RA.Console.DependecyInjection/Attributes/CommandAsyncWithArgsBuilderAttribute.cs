using RA.Console.DependecyInjection.Args;

namespace RA.Console.DependecyInjection.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CommandAsyncWithArgsBuilderAttribute<TArgsBuilder>(params string[] commands) :
        CommandAsyncAttribute(commands),
        ICommandWithArgsBuilderAttribute<TArgsBuilder> where TArgsBuilder : IArgsBuilder
    { }
}