using RA.Utilities.Extensions;

namespace RA.Utilities.Test.Extensions
{
    public class TaskExtensionsTest
    {
        [Fact]
        public async Task DelayOrCompleted_WhenTaskCompletesBeforeDelay_ShouldCompleteImmediately()
        {
            // Arrange
            var task = Task.Delay(100);
            var delayMilliseconds = 200;

            // Act
            await task.DelayOrCompleted(delayMilliseconds);

            // Assert
            Assert.True(task.IsCompleted);
        }

        [Fact]
        public async Task DelayOrCompleted_WhenTaskCompletesAfterDelay_ShouldContinueOnDelay()
        {
            // Arrange
            var task = Task.Delay(200);
            var delayMilliseconds = 100;

            // Act
            await task.DelayOrCompleted(delayMilliseconds);

            // Assert
            Assert.False(task.IsCompleted);
        }
    }
}
