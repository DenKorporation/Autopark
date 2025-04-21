using System.Reflection;
using Autopark.Common.Web.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Autopark.Common.Web.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddAutoparkAutoMapper(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddScoped<IClaimsTransformation, OpenIdClaimsTransformer>();
        services.AddHttpContextAccessor();
        
        services.AddAutoMapper(
            (m) =>
            {
                m.AllowNullCollections = true;
            },
            assemblies.Length == 0 ? AppDomain.CurrentDomain.GetAssemblies() : assemblies);

        return services;
    }
}
