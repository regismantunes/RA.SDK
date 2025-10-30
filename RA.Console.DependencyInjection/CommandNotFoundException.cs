namespace RA.Console.DependencyInjection
{
    /// <summary>
    /// Exception thrown when a requested command cannot be found.
    /// </summary>
    public class CommandNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandNotFoundException"/> class.
        /// </summary>
        public CommandNotFoundException() : base()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandNotFoundException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public CommandNotFoundException(string? message) : base(message)
        { }
    }
}