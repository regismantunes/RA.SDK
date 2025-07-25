using System;
using System.Collections.Generic;

namespace RA.Utilities.ErrorHandling
{
    /// <summary>
    /// A base class for custom error handler that allows ignoring specific exception types.
    /// </summary>
    /// <param name="ignoreException">Types of exceptions to ignore</param>
    public abstract class CustomErrorHandlerBase(params Type[] ignoreException)
    {
        private readonly ExceptionTypesCollection _ignoreExceptions = new(ignoreException);

        /// <summary>
        /// Adds an exception type to the list of ignored exceptions.
        /// </summary>
        /// <param name="exceptionType">The type of exception to ignore.</param>
        public void Ignore(Type exceptionType)
        {
            _ignoreExceptions.Add(exceptionType);
        }

        /// <summary>
        /// Removes an exception type from the list of ignored exceptions.
        /// </summary>
        /// <param name="exceptionType">The type of exception to stop ignoring.</param>
        public void NotIgnore(Type exceptionType)
        {
            _ignoreExceptions.Remove(exceptionType);
        }

        /// <summary>
        /// Check if the exception type will be ignored.
        /// </summary>
        /// <param name="exceptionType">The type of exception to check.</param>
        /// <returns>Returns true if the exception type is in the list of ignored exceptions or if the exception type inherits from some exception type in the list</returns>
        public bool IsIgnored(Type exceptionType)
        {
            return _ignoreExceptions.Contains(exceptionType);
        }

        /// <summary>
        /// Clears the list of ignored exceptions.
        /// </summary>
        public void ClearIgnored()
        {
            _ignoreExceptions.Clear();
        }

        /// <summary>
        /// Gets the list of ignored exception types.
        /// </summary>
        /// <returns>A read-only list of types that are currently ignored.</returns>
        public IReadOnlyList<Type> GetIgnoredExceptions()
        {
            return _ignoreExceptions.ToList();
        }
    }
}
