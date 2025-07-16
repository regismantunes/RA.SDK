using System;
using System.Threading.Tasks;

namespace RA.Utilities.Extensions
{
    public static class TaskExtensions
    {
        public static async Task DelayOrCompleted(this Task task, int millisecondsDelay)
        {
            ArgumentNullException.ThrowIfNull(task);
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(millisecondsDelay, 0, nameof(millisecondsDelay));
            
            var delayTask = Task.Delay(millisecondsDelay);
            await Task.WhenAny(task, delayTask);
        }
    }
}
