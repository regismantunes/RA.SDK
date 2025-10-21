namespace RA.Console.DependecyInjection
{
    public class CommandNotFoundException : Exception
    {
        public CommandNotFoundException() : base()
        { }

        public CommandNotFoundException(string? message) : base(message)
        { }
    }
}