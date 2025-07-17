using System;

namespace RA.Utilities.Output
{
    /// <summary>
    /// A factory class for creating output instances.
    /// </summary>
    public static class OutputFactory
    {
        /// <summary>
        /// Get the appropriate output instance based on the specified type.
        /// </summary>
        /// <param name="type">
        /// The type of output to create.
        /// </param>
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
