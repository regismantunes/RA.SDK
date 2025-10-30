namespace RA.Console.DependencyInjection
{
    /// <summary>
    /// Exception thrown when multiple commands are defined for the same alias.
    /// </summary>
    public class MultipleCommandsDefinedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleCommandsDefinedException"/> class.
        /// </summary>
        public MultipleCommandsDefinedException() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleCommandsDefinedException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MultipleCommandsDefinedException(string? message) : base(message) { }
    }
}
