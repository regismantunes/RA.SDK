using System;
using System.Collections.Generic;
using System.Linq;

namespace RA.Utilities.ErrorHandling
{
    internal sealed class ExceptionTypesCollection(params Type[] exceptionTypes)
    {
        private readonly List<Type> _exceptionTypes = ValidateExceptionTypes(exceptionTypes);

        public void Add(Type exceptionType)
        {
            ThrowIfNotExceptionType(exceptionType);

            if (!_exceptionTypes.Contains(exceptionType))
                _exceptionTypes.Add(exceptionType);
        }

        public void Remove(Type exceptionType)
        {
            ThrowIfNotExceptionType(exceptionType);

            _exceptionTypes.Remove(exceptionType);
        }

        public bool Contains(Type exceptionType)
        {
            ThrowIfNotExceptionType(exceptionType);

            return _exceptionTypes.Any(type => type.IsInstanceOfType(exceptionType));
        }

        public void Clear()
        {
            _exceptionTypes.Clear();
        }

        public IReadOnlyList<Type> ToList()
        {
            return _exceptionTypes.AsReadOnly();
        }

        private static void ThrowIfNotExceptionType(Type type)
        {
            ArgumentNullException.ThrowIfNull(type);

            // Check if the type is Exception itself or derives from Exception
            if (!type.IsSubclassOf(typeof(Exception)) && type != typeof(Exception))
                throw new ArgumentException($"Type '{type.FullName}' is not an Exception type.");
        }

        private static List<Type> ValidateExceptionTypes(IEnumerable<Type> exceptionTypes)
        {
            if (exceptionTypes == null)
                return [];

            foreach (var type in exceptionTypes)
                ThrowIfNotExceptionType(type);

            return exceptionTypes.ToList();
        }
    }
}
