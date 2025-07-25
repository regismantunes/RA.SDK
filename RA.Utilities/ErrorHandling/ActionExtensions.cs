using System;

namespace RA.Utilities.ErrorHandling
{
    public static class ActionExtensions
    {
        public static void TryExecute(this Action action, Action<Exception>? onError = null, params Type[] ignoreException)
        {
            new CustomErrorHandling(onError, ignoreException)
                .TryExecute(action);
        }

        public static void TryExecute<T>(this Action action, T exceptionParameter, Action<T,Exception> onError, params Type[] ignoreException)
        {
            new CustomErrorHandling<T>(onError, ignoreException)
                .TryExecute(action, exceptionParameter);
        }
    }
}
