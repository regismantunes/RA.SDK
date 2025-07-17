using System.Diagnostics;

namespace RA.Utilities.Extensions
{
    public static class StopwatchExtensions
    {
        /// <summary>
        /// Returns a string representation of the elapsed time in milliseconds, seconds, or minutes.
        /// </summary>
        /// <param name="stopwatch">
        /// The instance to get the elapsed time from.
        /// </param>
        public static string GetElapsedTimeText(this Stopwatch stopwatch)
        {
            var totalTime = (double)stopwatch.ElapsedMilliseconds;
            var timeType = "milliseconds";

            if (totalTime > 999d)
            {
                totalTime /= 1000d;
                timeType = "seconds";

                if (totalTime > 59d)
                {
                    totalTime /= 60d;
                    timeType = "minutes";
                }

                return $"{totalTime:N3} {timeType}";
            }

            return $"{totalTime} {timeType}";
        }
    }
}
