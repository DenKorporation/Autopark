using FS.Keycloak.RestApiClient.Model;

namespace Autopark.Common.Bl.Services.Keycloak.Interfaces;

public interface IUserService
{
    Task<UserRepresentation?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
    
    Task<IList<GroupRepresentation>> GetUserGroupsAsync(Guid userId, CancellationToken cancellationToken = default);
    
    Task<IList<GroupRepresentation>> GetUserGroupByPrefixAsync(Guid userId, string groupPrefix, CancellationToken cancellationToken = default);

    Task AddUserToGroupAsync(Guid userId, Guid groupId, CancellationToken cancellationToken = default);

    Task RemoveUserFromGroupAsync(Guid userId, Guid groupId, CancellationToken cancellationToken = default);
    
    Task CreateUserAsync(UserRepresentation userRequest, CancellationToken cancellationToken = default);

    Task ResetUserPasswordAsync(Guid userId, CredentialRepresentation request, CancellationToken cancellationToken = default);
}
