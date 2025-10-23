using RA.Console.DependencyInjection.Args;

namespace RA.Console.DependencyInjection.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CommandWithArgsBuilderAttribute<TArgsBuilder>(params string[] commands) :
        CommandAttribute(commands),
        ICommandWithArgsBuilderAttribute<TArgsBuilder> where TArgsBuilder : IArgsBuilder
    { }
}