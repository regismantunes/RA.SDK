namespace RA.Console.DependencyInjection.Args
{
    /// <summary>
    /// Exception thrown when command-line arguments fail validation.
    /// </summary>
    /// <param name="message">The validation error message.</param>
    public class ArgsValidationException(string message) : Exception(message) { }
}
