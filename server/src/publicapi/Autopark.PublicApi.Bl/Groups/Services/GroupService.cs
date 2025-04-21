using Autopark.Common.Attributes;
using Autopark.Common.Bl.Services.Keycloak.Interfaces;
using Autopark.Common.Mapping;
using Autopark.PublicApi.Bl.Groups.Services.Interfaces;
using Autopark.PublicApi.Shared.Groups.Dto;
using FluentResults;
using FS.Keycloak.RestApiClient.Model;

namespace Autopark.PublicApi.Bl.Groups.Services;

[ServiceAsInterfaces]
public class GroupService(
    IMapper<GroupRepresentation, GroupResponse> groupMapper,
    IGroupService groupService,
    IUserService userService) : IGroupsService
{
    public async Task<Result<IList<GroupResponse>>> GetAllUserGroupsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var userGroups = await userService.GetUserGroupsAsync(userId, cancellationToken);

        var parentIds = userGroups.Select(x => x.ParentId).Distinct().ToList();
        
        var result = new List<GroupResponse>();

        foreach (var parentId in parentIds)
        {
            var group = await groupService.GetGroupByIdAsync(Guid.Parse(parentId), cancellationToken: cancellationToken);
            
            result.Add(groupMapper.Map(group));
        }
        
        return result;
    }
}
