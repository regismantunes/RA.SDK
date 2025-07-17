namespace RA.Utilities.Output
{
    /// <summary>
    /// Interface for output operations.
    /// </summary>
    public interface IOutput
    {
        /// <summary>
        /// Writes a line of text to the output.
        /// </summary>
        /// <param name="message">
        /// The message to write to the output.
        /// </param>
        public void WriteLine(string message);

        /// <summary>
        /// Clear all the previous output messages.
        /// </summary>
        public void Clear();

        /// <summary>
        /// Clears the last line of output.
        /// </summary>
        public void ClearLine();
    }
}
