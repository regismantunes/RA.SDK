namespace RA.Console.DependecyInjection.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class ParameterAttribute(string name) : Attribute
    {
        public string Name { get; } = name;

        public bool IsCaseSensitive { get; init; } = false;
    }
}
