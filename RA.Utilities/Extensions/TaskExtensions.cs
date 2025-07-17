using System;
using System.Threading.Tasks;

namespace RA.Utilities.Extensions
{
    public static class TaskExtensions
    {
        /// <summary>
        /// Wait for the task to complete or delay for the specified milliseconds. Whichever comes first.
        /// </summary>
        /// <param name="task">
        /// The task to wait for.
        /// </param>
        /// <param name="millisecondsDelay">
        /// The number of milliseconds to wait before timing out.
        /// </param>
        public static async Task DelayOrCompleted(this Task task, int millisecondsDelay)
        {
            ArgumentNullException.ThrowIfNull(task);
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(millisecondsDelay, 0, nameof(millisecondsDelay));
            
            var delayTask = Task.Delay(millisecondsDelay);
            await Task.WhenAny(task, delayTask);
        }
    }
}
