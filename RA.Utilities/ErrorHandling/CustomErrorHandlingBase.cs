using System;
using System.Collections.Generic;

namespace RA.Utilities.ErrorHandling
{
    public abstract class CustomErrorHandlingBase(params Type[] ignoreException)
    {
        private readonly ExceptionTypesCollection _ignoreExceptions = new(ignoreException);

        public void Ignore(Type exceptionType)
        {
            _ignoreExceptions.Add(exceptionType);
        }

        public void NotIgnore(Type exceptionType)
        {
            _ignoreExceptions.Remove(exceptionType);
        }

        public bool IsIgnored(Type exceptionType)
        {
            return _ignoreExceptions.Contains(exceptionType);
        }

        public void ClearIgnored()
        {
            _ignoreExceptions.Clear();
        }

        public IReadOnlyList<Type> GetIgnoredExceptions()
        {
            return _ignoreExceptions.ToList();
        }
    }
}
