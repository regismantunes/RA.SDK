using System;

namespace RA.Utilities.ErrorHandling
{
    public class CustomErrorHandling<T>(Action<T, Exception>? onError = null, params Type[] ignoreException)
    {
        private readonly Action<T, Exception>? _onError = onError;
        private readonly ExceptionTypesCollection _exceptions = new(ignoreException);

        public void TryExecute(Action action, T exceptionParameter)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (_exceptions.Contains(ex.GetType()))
                    return;
                
                _onError?.Invoke(exceptionParameter, ex);
            }
        }
    }

    public class CustomErrorHandling(Action<Exception> onError, params Type[] ignoreException)
    {
        private readonly Action<Exception>? _onError = onError;
        private readonly ExceptionTypesCollection _exceptions = new(ignoreException);

        public void TryExecute(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (_exceptions.Contains(ex.GetType()))
                    return;

                _onError?.Invoke(ex);
            }
        }
    }
}
