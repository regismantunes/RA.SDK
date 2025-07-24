using System;
using System.Reflection.Metadata;

namespace RA.Utilities.ErrorHandling
{
    public static class ActionExtensions
    {
        public static void TryExecute(this Action action, Action<Exception>? onError = null, params Type[] ignoreException)
        {
            ArgumentNullException.ThrowIfNull(action);

            var ignoreExceptionCollection = new ExceptionTypesCollection(ignoreException);

            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (ignoreExceptionCollection.Contains(ex.GetType()))
                    return;

                onError?.Invoke(ex);
            }
        }

        public static void TryExecute<T>(this Action action, T exceptionParameter, Action<T,Exception>? onError = null, params Type[] ignoreException)
        {
            ArgumentNullException.ThrowIfNull(action);

            var ignoreExceptionCollection = new ExceptionTypesCollection(ignoreException);

            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (ignoreExceptionCollection.Contains(ex.GetType()))
                    return;

                onError?.Invoke(exceptionParameter, ex);
            }
        }
    }
}
