using RA.Console.DependencyInjection.Args;

namespace RA.Console.DependencyInjection.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    /// <summary>
    /// Declares a console command for a method and specifies a synchronous arguments builder type.
    /// </summary>
    /// <typeparam name="TArgsBuilder">The arguments builder type implementing <see cref="IArgsBuilder"/>.</typeparam>
    public class CommandWithArgsBuilderAttribute<TArgsBuilder>(params string[] commands) :
        CommandAttribute(commands),
        ICommandWithArgsBuilderAttribute<TArgsBuilder> where TArgsBuilder : IArgsBuilder
    { }
}