namespace RA.Console.DependecyInjection
{
    public class MultipleCommandsDefinedException : Exception
    {
        public MultipleCommandsDefinedException() : base() { }

        public MultipleCommandsDefinedException(string? message) : base(message) { }
    }
}
