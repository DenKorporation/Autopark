using Autopark.Common.Attributes;
using Autopark.Common.Bl.Services.Keycloak.Interfaces;
using Autopark.Common.Configuration;
using Autopark.Common.Extensions;
using FS.Keycloak.RestApiClient.Api;
using FS.Keycloak.RestApiClient.Model;
using Microsoft.Extensions.Options;

namespace Autopark.Common.Bl.Services.Keycloak;

[ServiceAsInterfaces]
public class GroupService(
    IGroupsApiAsync groupsApi,
    IOptions<IdentityConfiguration> identityConfig) 
    : IGroupService
{
    private readonly string _realm = identityConfig.Value.Realm;

    /// <inheritdoc/>
    public async Task<List<GroupRepresentation>> GetGroupsAsync(int first = 0, int max = 20, bool deepLoad = false, CancellationToken cancellationToken = default)
    {
        var groups = await groupsApi.GetGroupsAsync(_realm, first: first, max: max, cancellationToken: cancellationToken);

        if (deepLoad)
        {
            foreach (var group in groups)
            {
                await LoadGroupChildrenAsync(group, cancellationToken);
            }
        }
        
        return groups;
    }

    public async Task<IList<UserRepresentation>> GetGroupMembersAsync(Guid groupId, CancellationToken cancellationToken = default)
    {
        var group = await GetGroupByIdAsync(groupId, deepLoad: true, cancellationToken);

        if (group is null)
        {
            return new List<UserRepresentation>();
        }
        
        var result = new List<UserRepresentation>();

        foreach (var subGroup in group.SubGroups)
        {
            var tempUsers = await RequestExtensions.GetAllDataAsync<UserRepresentation>(
                async (first, max, ct) =>
                    await groupsApi.GetGroupsMembersByGroupIdAsync(
                        _realm,
                        subGroup.Id,
                        briefRepresentation: false,
                        first,
                        max,
                        ct));
            
            result = result.Union(tempUsers).ToList();
        }

        return result;
    }

    public async Task<GroupRepresentation?> GetGroupByIdAsync(Guid id, bool deepLoad = false, CancellationToken cancellationToken = default)
    {
        var group = await groupsApi.GetGroupsByGroupIdAsync(_realm, id.ToString(), cancellationToken: cancellationToken);

        if (deepLoad && group is not null)
        {
            await LoadGroupChildrenAsync(group, cancellationToken);
        }

        return group;
    }

    public async Task<GroupRepresentation?> GetGroupByNameAsync(string name, bool deepLoad = false, CancellationToken cancellationToken = default)
    {
        var groups = await groupsApi.GetGroupsAsync(_realm, exact: true, search: name, populateHierarchy: false, cancellationToken: cancellationToken);
        
        var result = groups?.FirstOrDefault();

        if (deepLoad && result is not null)
        {
            await LoadGroupChildrenAsync(result, cancellationToken);
        }
        
        return result;
    }

    public async Task CreateGroupAsync(string name, CancellationToken cancellationToken = default)
    {
        await groupsApi.PostGroupsAsync(
            _realm,
            new GroupRepresentation()
            {
                Name = name,
            },
            cancellationToken);

        var group = await GetGroupByNameAsync(name, deepLoad: true, cancellationToken);
        var groupRoles = group!.SubGroups.Select(x => x.Name).ToList();
        
        var roles = RoleExtensions.GetAllRoles();

        foreach (var role in roles)
        {
            if (groupRoles.Contains(role))
            {
                continue;
            }

            await groupsApi.PostGroupsChildrenByGroupIdAsync(
                _realm,
                group!.Id,
                new GroupRepresentation()
                {
                    Name = role,
                },
                cancellationToken);
        }
    }

    private async Task LoadGroupChildrenAsync(GroupRepresentation group, CancellationToken cancellationToken)
    {
        var children = await RequestExtensions.GetAllDataAsync<GroupRepresentation>(
            async (first, max, ct) => await groupsApi.GetGroupsChildrenByGroupIdAsync(
                _realm,
                group.Id,
                first: first,
                max: max,
                cancellationToken: ct),
            cancellationToken: cancellationToken);

        group.SubGroups = children.ToList();
    }
}
