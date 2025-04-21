using Autopark.Common.Attributes;
using Autopark.Common.Bl.Services.Keycloak.Interfaces;
using Autopark.Common.Configuration;
using Autopark.Common.Extensions;
using FS.Keycloak.RestApiClient.Api;
using FS.Keycloak.RestApiClient.Model;
using Microsoft.Extensions.Options;

namespace Autopark.Common.Bl.Services.Keycloak;

[ServiceAsInterfaces]
public class UserService(
    IUsersApiAsync usersApi,
    IOptions<IdentityConfiguration> identityConfig) 
    : IUserService
{
    private readonly string _realm = identityConfig.Value.Realm;

    public async Task<UserRepresentation?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var users = await usersApi.GetUsersAsync(_realm, briefRepresentation: false, email, exact: true, cancellationToken: cancellationToken);

        return users?.FirstOrDefault();
    }

    public async Task<IList<GroupRepresentation>> GetUserGroupsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await RequestExtensions.GetAllDataAsync<GroupRepresentation>(
            async (first, max, ct) => await usersApi.GetUsersGroupsByUserIdAsync(
                _realm,
                userId.ToString(),
                first: first,
                max: max,
                cancellationToken: ct),
            cancellationToken: cancellationToken);
    }

    public async Task<IList<GroupRepresentation>> GetUserGroupByPrefixAsync(Guid userId, string groupPrefix, CancellationToken cancellationToken = default)
    {
        var groups = await GetUserGroupsAsync(userId, cancellationToken);

        var prefix = $"/{groupPrefix}";

        return groups.Where(x => x.Path.StartsWith(prefix)).ToList();
    }

    public async Task AddUserToGroupAsync(Guid userId, Guid groupId, CancellationToken cancellationToken = default)
    {
        await usersApi.PutUsersGroupsByUserIdAndGroupIdAsync(
            _realm,
            userId.ToString(),
            groupId.ToString(),
            cancellationToken);
    }

    public async Task RemoveUserFromGroupAsync(Guid userId, Guid groupId, CancellationToken cancellationToken = default)
    {
        await usersApi.DeleteUsersGroupsByUserIdAndGroupIdAsync(
            _realm,
            userId.ToString(),
            groupId.ToString(),
            cancellationToken);
    }

    public async Task CreateUserAsync(UserRepresentation userRequest, CancellationToken cancellationToken = default)
    {
        await usersApi.PostUsersAsync(_realm, userRequest, cancellationToken);
    }

    public async Task ResetUserPasswordAsync(Guid userId, CredentialRepresentation request, CancellationToken cancellationToken = default)
    {
        await usersApi.PutUsersResetPasswordByUserIdAsync(_realm, userId.ToString(), request, cancellationToken);
    }
}
