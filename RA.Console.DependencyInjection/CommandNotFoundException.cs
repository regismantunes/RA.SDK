namespace RA.Console.DependencyInjection
{
    public class CommandNotFoundException : Exception
    {
        public CommandNotFoundException() : base()
        { }

        public CommandNotFoundException(string? message) : base(message)
        { }
    }
}