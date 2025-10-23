namespace RA.Console.DependencyInjection
{
    public class NoCommandsDefinedException : Exception
    {
        public NoCommandsDefinedException() { }

        public NoCommandsDefinedException(string? message) : base(message) { }
    }
}