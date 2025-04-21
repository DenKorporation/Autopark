using FS.Keycloak.RestApiClient.Model;

namespace Autopark.Common.Bl.Services.Keycloak.Interfaces;

public interface IUserRoleService
{
    Task<IList<RoleRepresentation>> GetAvailableRolesAsync(Guid userId, CancellationToken cancellationToken);

    Task<IList<RoleRepresentation>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken = default);

    Task AddRoleToUserAsync(Guid userId, List<RoleRepresentation> roles, CancellationToken cancellationToken = default);

    Task RemoveRoleFromUserAsync(Guid userId, List<RoleRepresentation> roles, CancellationToken cancellationToken = default);
}
