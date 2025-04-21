using FS.Keycloak.RestApiClient.Model;

namespace Autopark.Common.Bl.Services.Keycloak.Interfaces;

public interface IGroupService
{
    Task<List<GroupRepresentation>> GetGroupsAsync(
        int first = 0,
        int max = 20,
        bool deepLoad = false,
        CancellationToken cancellationToken = default);

    Task<IList<UserRepresentation>> GetGroupMembersAsync(Guid groupId, CancellationToken cancellationToken = default);

    Task<GroupRepresentation?> GetGroupByIdAsync(Guid id, bool deepLoad = false, CancellationToken cancellationToken = default);

    Task<GroupRepresentation?> GetGroupByNameAsync(string name, bool deepLoad = false, CancellationToken cancellationToken = default);

    Task CreateGroupAsync(string name, CancellationToken cancellationToken = default);
}
