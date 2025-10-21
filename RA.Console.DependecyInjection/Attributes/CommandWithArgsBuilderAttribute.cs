using RA.Console.DependecyInjection.Args;

namespace RA.Console.DependecyInjection.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CommandWithArgsBuilderAttribute<TArgsBuilder>(params string[] commands) :
        CommandAttribute(commands),
        ICommandWithArgsBuilderAttribute<TArgsBuilder> where TArgsBuilder : IArgsBuilder
    { }
}