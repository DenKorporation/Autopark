using Autopark.Common.Attributes;
using Autopark.Common.Bl.Services.Keycloak.Interfaces;
using Autopark.Common.Configuration;
using FS.Keycloak.RestApiClient.Api;
using FS.Keycloak.RestApiClient.Model;
using Microsoft.Extensions.Options;

namespace Autopark.Common.Bl.Services.Keycloak;

[ServiceAsInterfaces]
public class UserRoleService(
    IRoleMapperApiAsync rolesApiAsync,
    IOptions<IdentityConfiguration> identityConfig)
    : IUserRoleService
{
    private readonly string _realm = identityConfig.Value.Realm;

    public async Task<IList<RoleRepresentation>> GetAvailableRolesAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await rolesApiAsync.GetUsersRoleMappingsRealmAvailableByUserIdAsync(
            _realm,
            userId.ToString(),
            cancellationToken);
    }

    public async Task<IList<RoleRepresentation>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await rolesApiAsync.GetUsersRoleMappingsRealmByUserIdAsync(_realm, userId.ToString(), cancellationToken);
    }

    public async Task AddRoleToUserAsync(Guid userId, List<RoleRepresentation> roles, CancellationToken cancellationToken = default)
    {
        await rolesApiAsync.PostUsersRoleMappingsRealmByUserIdAsync(_realm, userId.ToString(), roles, cancellationToken);
    }

    public async Task RemoveRoleFromUserAsync(Guid userId, List<RoleRepresentation> roles, CancellationToken cancellationToken = default)
    {
        await rolesApiAsync.DeleteUsersRoleMappingsRealmByUserIdAsync(_realm, userId.ToString(), roles, cancellationToken);
    }
}
