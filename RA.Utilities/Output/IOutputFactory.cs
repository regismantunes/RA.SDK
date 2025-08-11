namespace RA.Utilities.Output
{
    public interface IOutputFactory
    {
        /// <summary>
        /// Get the appropriate output instance based on the specified type.
        /// </summary>
        /// <param name="type">
        /// The type of output to create.
        /// </param>
        IOutput CreateOutput(OutputType outputType);
    }
}