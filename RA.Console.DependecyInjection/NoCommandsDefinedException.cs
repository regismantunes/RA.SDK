namespace RA.Console.DependecyInjection
{
    public class NoCommandsDefinedException : Exception
    {
        public NoCommandsDefinedException() { }

        public NoCommandsDefinedException(string? message) : base(message) { }
    }
}