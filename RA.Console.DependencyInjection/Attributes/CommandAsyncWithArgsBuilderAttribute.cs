using RA.Console.DependencyInjection.Args;

namespace RA.Console.DependencyInjection.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    /// <summary>
    /// Declares an asynchronous console command for a method and specifies a synchronous arguments builder type.
    /// </summary>
    /// <typeparam name="TArgsBuilder">The arguments builder type implementing <see cref="Args.IArgsBuilder"/>.</typeparam>
    /// <param name="commands">The command aliases that invoke the method.</param>
    public class CommandAsyncWithArgsBuilderAttribute<TArgsBuilder>(params string[] commands) :
        CommandAsyncAttribute(commands),
        ICommandWithArgsBuilderAttribute<TArgsBuilder> where TArgsBuilder : IArgsBuilder
    { }
}