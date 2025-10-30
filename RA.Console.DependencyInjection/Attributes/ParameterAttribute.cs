namespace RA.Console.DependencyInjection.Attributes
{
    /// <summary>
    /// Specifies a custom name and options for a command method parameter.
    /// </summary>
    /// <param name="name">The expected name in the parsed arguments.</param>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class ParameterAttribute(string name) : Attribute
    {
        /// <summary>
        /// Gets the expected argument name for the parameter.
        /// </summary>
        public string Name { get; } = name;

        /// <summary>
        /// Indicates whether argument name matching is case sensitive.
        /// </summary>
        public bool IsCaseSensitive { get; init; } = false;
    }
}
