using System.Reflection;

namespace RA.Console.DependencyInjection.Attributes
{
    /// <summary>
    /// Declares a console command for a method.
    /// </summary>
    /// <param name="commands">The command aliases that invoke the method.</param>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CommandAttribute(params string[] commands) : Attribute
    {
        /// <summary>
        /// The set of command aliases that invoke the method.
        /// </summary>
        public string[] Commands { get; } = commands;
        /// <summary>
        /// The command description used in help output.
        /// </summary>
        public string Description { get; init; } = string.Empty;
        /// <summary>
        /// An example usage string for the command.
        /// </summary>
        public string Example { get; init; } = string.Empty;
        /// <summary>
        /// Indicates whether this command should be hidden from help output.
        /// </summary>
        public bool Hide { get; init; } = false;
        /// <summary>
        /// The group name used to organize commands in help output.
        /// </summary>
        public string? Group { get; init; }
        /// <summary>
        /// Ordering value used to sort commands within a group.
        /// </summary>
        public int Order { get; init; } = 0;

        /// <summary>
        /// Validates the decorated method signature. The method should return int.
        /// </summary>
        /// <param name="method">The method to validate.</param>
        /// <exception cref="InvalidOperationException">Thrown when the method has an invalid return type.</exception>
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