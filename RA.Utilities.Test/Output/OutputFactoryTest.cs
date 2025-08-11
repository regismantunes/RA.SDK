using RA.Utilities.Output;

namespace RA.Utilities.Test.Output
{
    public class OutputFactoryTest
    {
        [Fact]
        public void CreateOutput_WhenOutputTypeIsConsole_ShouldReturnOutputInstanceOfConsoleOutput()
        {
            // Arrange
            var outputFactory = new OutputFactory();
            // Act
            var output = outputFactory.CreateOutput(OutputType.Console);
            // Assert
            Assert.NotNull(output);
            Assert.IsType<ConsoleOutput>(output);
        }
    }
}
