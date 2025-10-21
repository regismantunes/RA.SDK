using System.Reflection;

namespace RA.Console.DependecyInjection.Args
{
    internal static class DefaultArgsBuilder
    {
        public static IDictionary<string, object> Build(MethodInfo methodInfo, string[] args)
        {
            ParameterInfo[] methodParameters = methodInfo.GetParameters();
            var methodArgs = new Dictionary<string, object>();
            for (var i = 1; i < args.Length; i++)
            {
                if (methodParameters.Length <= i - 1)
                    break;
                var parameterName = methodParameters[i - 1].Name ?? throw new InvalidOperationException($"There was an error getting parameter name from method '{methodInfo.Name}' of class '{methodInfo.DeclaringType?.Name}'.");
                methodArgs.Add(parameterName, args[i]);
            }
            return methodArgs;
        }
    }
}
