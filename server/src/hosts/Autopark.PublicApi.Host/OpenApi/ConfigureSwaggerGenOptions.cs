using Asp.Versioning.ApiExplorer;
using Autopark.Common.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Autopark.PublicApi.Host.OpenApi;

public class ConfigureSwaggerGenOptions(IApiVersionDescriptionProvider provider, IConfiguration configuration)
    : IConfigureNamedOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            var openApiInfo = new OpenApiInfo
            {
                Title = $"AutoPark.Public.API v{description.ApiVersion}",
                Version = description.ApiVersion.ToString(),
            };
            options.SwaggerDoc(description.GroupName, openApiInfo);
        }

        var config = configuration.GetSection(nameof(IdentityConfiguration)).Get<IdentityConfiguration>()!;

        var baseUri = $"{config.BaseUrl.TrimEnd('/')}/realms/{config.Realm}";
        
        var tokenUri = new Uri($"{baseUri}/{config.TokenEndpoint.TrimStart('/')}");
        var authorityUri = new Uri($"{baseUri}/{config.AuthorizationEndpoint.TrimStart('/')}");
        
        options.AddSecurityDefinition(
            "OAuth2",
            new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    Implicit = new OpenApiOAuthFlow()
                    {
                        RefreshUrl = tokenUri,
                        TokenUrl = tokenUri,
                        AuthorizationUrl = authorityUri,
                        Scopes = new Dictionary<string, string>
                        {
                            { "openid", "openid" },
                            { "profile", "profile" },
                            { config.Scope, config.Scope },
                            { "offline_access", "offline_access" },
                        },
                    },
                },
            });

        options.AddSecurityRequirement(
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "OAuth2",
                        },
                        Name = "Bearer",
                        Scheme = "Bearer",
                    },
                    new string[] { }
                },
            });
    }

    public void Configure(string? name, SwaggerGenOptions options)
    {
        Configure(options);
    }
}