using System;

namespace RA.Utilities.Output
{
    public static class OutputFactory
    {
        public static IOutput GetOutput(OutputType type)
        {
            return type switch
            {
                OutputType.Console => new ConsoleOutput(),
                _ => throw new ArgumentException("Invalid output type"),
            };
        }
    }
}
