using System;

namespace RA.Utilities.ErrorHandling
{
    /// <summary>
    /// Class for custom error handler that allows to provide a default error statement, with a generic parameter that help to identify the error origin, and ignoring specific exception types.
    /// </summary>
    /// <param name="onError">The action to be handled if some not ignored exception occours</param>
    /// <param name="ignoreException">Types of exceptions to ignore. If the exception type is not in this list, it will be handled by the onError action.</param>
    public sealed class CustomErrorHandler<T>(Action<T, Exception> onError, params Type[] ignoreException)
        : CustomErrorHandlerBase(ignoreException)
    {
        private readonly Action<T, Exception>? _onError = onError;

        /// <summary>
        /// Try to execute an action and handle the onError action with T as parameter if some not ignored exception occours.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="exceptionParameter">Parameter to be passed to the onError action when an exception occurs.</param>
        /// <returns>Returns true if the action executed successfully, false if an exception occurred that was not ignored.</returns>
        public bool TryExecute(Action action, T exceptionParameter)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (!IsIgnored(ex.GetType()))
                {
                    _onError?.Invoke(exceptionParameter, ex);
                    return false;
                }
            }

            return true;
        }
    }

    /// <summary>
    /// Class for custom error handler that allows to provide a default error statement and ignoring specific exception types.
    /// </summary>
    /// <param name="onError">The action to be handled if some not ignored exception occours</param>
    /// <param name="ignoreException">Types of exceptions to ignore. If the exception type is not in this list, it will be handled by the onError action.</param>
    public sealed class CustomErrorHandler(Action<Exception>? onError = null, params Type[] ignoreException)
        : CustomErrorHandlerBase(ignoreException)
    {
        private readonly Action<Exception>? _onError = onError;

        /// <summary>
        /// Try to execute an action and handle the onError action if some not ignored exception occours.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <returns>Returns true if the action executed successfully, false if an exception occurred that was not ignored.</returns>
        public bool TryExecute(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (!IsIgnored(ex.GetType()))
                {
                    _onError?.Invoke(ex);
                    return false;
                }
            }

            return true;
        }
    }
}
