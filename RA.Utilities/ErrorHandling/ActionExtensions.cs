using System;

namespace RA.Utilities.ErrorHandling
{
    public static class ActionExtensions
    {
        public static bool TryExecute(this Action action, Action<Exception>? onError = null, params Type[] ignoreException)
        {
            return new CustomErrorHandling(onError, ignoreException)
                .TryExecute(action);
        }

        public static bool TryExecute<T>(this Action action, T exceptionParameter, Action<T,Exception> onError, params Type[] ignoreException)
        {
            return new CustomErrorHandling<T>(onError, ignoreException)
                .TryExecute(action, exceptionParameter);
        }
    }
}
