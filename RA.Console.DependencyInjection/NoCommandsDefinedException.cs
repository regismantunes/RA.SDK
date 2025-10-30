namespace RA.Console.DependencyInjection
{
    /// <summary>
    /// Exception thrown when no commands are defined in the registered assemblies.
    /// </summary>
    public class NoCommandsDefinedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoCommandsDefinedException"/> class.
        /// </summary>
        public NoCommandsDefinedException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoCommandsDefinedException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public NoCommandsDefinedException(string? message) : base(message) { }
    }
}