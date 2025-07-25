using System;

namespace RA.Utilities.ErrorHandling
{
    public sealed class CustomErrorHandling<T>(Action<T, Exception> onError, params Type[] ignoreException)
        : CustomErrorHandlingBase(ignoreException)
    {
        private readonly Action<T, Exception>? _onError = onError;

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

    public sealed class CustomErrorHandling(Action<Exception>? onError = null, params Type[] ignoreException)
        : CustomErrorHandlingBase(ignoreException)
    {
        private readonly Action<Exception>? _onError = onError;

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
