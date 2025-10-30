using RA.Console.DependencyInjection.Args;

namespace RA.Console.DependencyInjection.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    /// <summary>
    /// Declares a console command for a method and specifies an asynchronous arguments builder type.
    /// </summary>
    /// <typeparam name="TArgsBuilderAsync">The async arguments builder type implementing <see cref="IArgsBuilderAsync"/>.</typeparam>
    public class CommandWithArgsBuilderAsyncAttribute<TArgsBuilderAsync>(params string[] commands) :
        CommandAttribute(commands),
        ICommandWithArgsBuilderAsyncAttribute<TArgsBuilderAsync> where TArgsBuilderAsync : IArgsBuilderAsync
    { }
}