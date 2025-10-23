using System.Reflection;

namespace RA.Console.DependencyInjection.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CommandAsyncAttribute(params string[] commands) : CommandAttribute(commands)
    {
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