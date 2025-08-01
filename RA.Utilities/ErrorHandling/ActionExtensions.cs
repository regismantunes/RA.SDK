using System;

namespace RA.Utilities.ErrorHandling
{
    public static class ActionExtensions
    {
        /// <summary>
        /// Try to execute an action and handle another action if some not ignored exception occours.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="onError">The action to be handled if some not ignored exception occours</param>
        /// <param name="ignoreException">Types of exceptions to ignore. If the exception type is not in this list, it will be handled by the onError action.</param>
        /// <returns>Returns true if the action executed successfully, false if an exception occurred that was not ignored.</returns>
        public static bool TryExecute(this Action action, Action<Exception>? onError = null, params Type[] ignoreException)
        {
            return new CustomErrorHandler(onError, ignoreException)
                .TryExecute(action);
        }

        /// <summary>
        /// Try to execute an action and handle another action with T as parameter if some not ignored exception occours.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="exceptionParameter">Parameter to be passed to the onError action when an exception occurs.</param>
        /// <param name="onError">The action to be handled if some not ignored exception occours</param>
        /// <param name="ignoreException">Types of exceptions to ignore. If the exception type is not in this list, it will be handled by the onError action.</param>
        /// <returns>Returns true if the action executed successfully, false if an exception occurred that was not ignored.</returns>
        public static bool TryExecute<T>(this Action action, T exceptionParameter, Action<T,Exception> onError, params Type[] ignoreException)
        {
            return new CustomErrorHandler<T>(onError, ignoreException)
                .TryExecute(action, exceptionParameter);
        }
    }
}
