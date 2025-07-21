using RA.Utilities.Extensions;
using System.Diagnostics;

namespace RA.Utilities.Test.Extensions
{
    public class StopwatchExtensionsTest
    {
        [Fact]
        public void GetElapsedTimeText_WhenSleepMilliseconds_ShouldReturnTheElapsedTimeInTextFormat()
        {
            // Arrange
            var stopwatch = new Stopwatch();

            // Act
            stopwatch.Start();
            Thread.Sleep(550); // Sleep for 550 miliseconds
            stopwatch.Stop();
            var elapsedTimeText = stopwatch.GetElapsedTimeText();

            // Assert
            var parts = elapsedTimeText.Split(' ');
            Assert.Equal(2, parts.Length);
            var milliseconds = double.Parse(parts[0], System.Globalization.CultureInfo.InvariantCulture);
            // The assert should ignore the small differences in the elapsed time due to the sleep duration
            Assert.True(milliseconds is >= 550 and < 600);
            Assert.Equal("milliseconds", parts[1]);
        }

        [Fact]
        public void GetElapsedTimeText_WhenSleepSeconds_ShouldReturnTheElapsedTimeInTextFormat()
        {
            // Arrange
            var stopwatch = new Stopwatch();

            // Act
            stopwatch.Start();
            Thread.Sleep(1500); // Sleep for 1.5 seconds
            stopwatch.Stop();
            var elapsedTimeText = stopwatch.GetElapsedTimeText();

            // Assert
            var parts = elapsedTimeText.Split(' ');
            Assert.Equal(2, parts.Length);
            var milliseconds = decimal.Parse(parts[0], System.Globalization.CultureInfo.InvariantCulture);
            // The assert should ignore the small differences in the elapsed time due to the sleep duration
            Assert.True(milliseconds is >= (decimal)1.5 and < (decimal)1.6);
            Assert.Equal("seconds", parts[1]);
        }

        [Fact]
        public void GetElapsedTimeText_WhenSleepMinutes_ShouldReturnTheElapsedTimeInTextFormat()
        {
            // Arrange
            var stopwatch = new Stopwatch();

            // Act
            stopwatch.Start();
            Thread.Sleep(90000); // Sleep for 1.5 minutes
            stopwatch.Stop();
            var elapsedTimeText = stopwatch.GetElapsedTimeText();

            // Assert
            var parts = elapsedTimeText.Split(' ');
            Assert.Equal(2, parts.Length);
            var milliseconds = decimal.Parse(parts[0], System.Globalization.CultureInfo.InvariantCulture);
            // The assert should ignore the small differences in the elapsed time due to the sleep duration
            Assert.True(milliseconds is >= (decimal)1.5 and < (decimal)1.6);
            Assert.Equal("minutes", parts[1]);
        }
    }
}