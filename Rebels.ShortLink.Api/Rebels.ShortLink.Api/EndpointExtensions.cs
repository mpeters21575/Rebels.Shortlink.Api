using System.Reflection;
using System.Runtime.CompilerServices;

namespace Rebels.ShortLink.Api;

public static class EndpointExtensions
{
    public static void MapEndpoints(this WebApplication app)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var endpointMappingMethods = assembly.GetTypes()
            .Where(t => !t.IsGenericType && !t.IsNested) 
            .SelectMany(t => t.GetMethods(BindingFlags.Static | BindingFlags.Public))
            .Where(m => m.IsDefined(typeof(ExtensionAttribute), inherit: false))
            .Where(m =>
            {
                var parameters = m.GetParameters();
                return parameters.Length == 1
                       && parameters[0].ParameterType == typeof(IEndpointRouteBuilder)
                       && m.ReturnType == typeof(IEndpointRouteBuilder);
            });

        foreach (var method in endpointMappingMethods)
        {
            method.Invoke(null, [app]);
        }
    }
}