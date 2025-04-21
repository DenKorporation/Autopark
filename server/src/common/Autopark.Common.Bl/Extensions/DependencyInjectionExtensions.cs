using Autopark.Common.Configuration;
using FS.Keycloak.RestApiClient.Api;
using FS.Keycloak.RestApiClient.Authentication.ClientFactory;
using FS.Keycloak.RestApiClient.Authentication.Flow;
using FS.Keycloak.RestApiClient.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ApiClientFactory = FS.Keycloak.RestApiClient.ClientFactory.ApiClientFactory;

namespace Autopark.Common.Bl.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddKeycloakApi(this IServiceCollection services)
    {
        services.AddKeycloakApi<IRoleMapperApiAsync, RoleMapperApi>();
        services.AddKeycloakApi<IUsersApiAsync, UsersApi>();
        services.AddKeycloakApi<IGroupsApiAsync, GroupsApi>();
        
        return services;
    }

    private static void AddKeycloakApi<TRegistration, TImplementation>(this IServiceCollection services)
        where TRegistration : class
        where TImplementation : TRegistration, IApiAccessor
    {
        services.AddScoped(CreateApi<TRegistration, TImplementation>);
    }

    private static TRegistration CreateApi<TRegistration, TImplementation>(IServiceProvider provider)
        where TRegistration : class
        where TImplementation : TRegistration, IApiAccessor
    {
        var identityConfig = provider.GetRequiredService<IOptions<IdentityConfiguration>>().Value;

        var credentials = new PasswordGrantFlow()
        {
            KeycloakUrl = identityConfig.BaseUrl,
            Realm = identityConfig.AdminConfiguration.Realm,
            UserName = identityConfig.AdminConfiguration.UserName,
            Password = identityConfig.AdminConfiguration.Password
        };

        var httpClient = AuthenticationHttpClientFactory.Create(credentials);
        var apiAccessor = ApiClientFactory.Create<TImplementation>(httpClient);

        return apiAccessor;
    }
}
