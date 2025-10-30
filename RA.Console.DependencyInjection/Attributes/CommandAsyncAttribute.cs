using System.Reflection;

namespace RA.Console.DependencyInjection.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    /// <summary>
    /// Declares an asynchronous console command for a method.
    /// </summary>
    /// <param name="commands">The command aliases that invoke the method.</param>
    public class CommandAsyncAttribute(params string[] commands) : CommandAttribute(commands)
    {
        /// <summary>
        /// Validates that the decorated method returns <see cref="Task{TResult}"/> of <see cref="int"/>.
        /// </summary>
        /// <param name="method">The method to validate.</param>
        /// <exception cref="InvalidOperationException">Thrown when the method has an invalid return type.</exception>
        public override void Validate(MethodInfo method)
        {
            var returnType = method.ReturnType;

            if (returnType != typeof(Task<int>))
            {
                throw new InvalidOperationException(
                    $"Method '{method.Name}' must return Task<int>.");
            }
        }
    }
}