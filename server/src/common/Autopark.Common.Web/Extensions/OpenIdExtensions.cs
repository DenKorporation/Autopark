using Autopark.Common.Bl.Extensions;
using Autopark.Common.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Autopark.Common.Web.Extensions;

public static class OpenIdExtensions
{
    public static IServiceCollection ConfigureOpenIdAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<IdentityConfiguration>()
            .Bind(configuration.GetSection(nameof(IdentityConfiguration)))
            .ValidateDataAnnotations();

        services.ConfigureAuthentication(configuration);
        services.AddKeycloakApi();

        return services;
    }

    private static IServiceCollection ConfigureAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var config = configuration.GetSection(nameof(IdentityConfiguration)).Get<IdentityConfiguration>()!;

        services.AddAuthentication(
                options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
            .AddJwtBearer(
                options =>
                {
                    options.MetadataAddress = $"{config.BaseUrl.TrimEnd('/')}/realms/{config.Realm}/{config.OIDCEndpoint.TrimStart('/')}";
                    options.RequireHttpsMetadata = false;
                    options.Audience = config.Audience;
                });

        return services;
    }
}
