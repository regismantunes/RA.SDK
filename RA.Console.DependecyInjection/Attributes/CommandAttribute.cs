using System.Reflection;

namespace RA.Console.DependecyInjection.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CommandAttribute(params string[] commands) : Attribute
    {
        public string[] Commands { get; } = commands;
        public string Description { get; init; } = string.Empty;
        public string Example { get; init; } = string.Empty;
        public bool Hide { get; init; } = false;
        public string? Group { get; init; }
        public int Order { get; init; } = 0;

        public virtual void Validate(MethodInfo method)
        {
            var returnType = method.ReturnType;

            if (returnType != typeof(int))
            {
                throw new InvalidOperationException(
                    $"Method '{method.Name}' must return Task<int>.");
            }
        }
    }
}