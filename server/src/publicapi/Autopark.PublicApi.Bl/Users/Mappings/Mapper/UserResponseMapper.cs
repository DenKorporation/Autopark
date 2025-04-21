using AutoMapper;
using Autopark.Common.Bl.Mappers;
using Autopark.Common.Bl.Services.Keycloak.Interfaces;
using Autopark.Common.Security;
using Autopark.PublicApi.Shared.Users.Dto;
using FS.Keycloak.RestApiClient.Model;

namespace Autopark.PublicApi.Bl.Users.Mappings.Mapper;

public class UserResponseMapper(
    IUserInfoProvider userInfoProvider,
    IUserService userService,
    IMapper mapper)
    : DefaultMapper<UserRepresentation, UserResponse>(mapper)
{
    private string GroupName => userInfoProvider.GetGroupName()!;
    public override void AfterMap(IEnumerable<UserRepresentation> inputQuery, IEnumerable<UserResponse> existingModels)
    {
        foreach (var model in existingModels)
        {
            var userGroups = userService.GetUserGroupByPrefixAsync(model.Id, GroupName).GetAwaiter().GetResult();

            model.Role = userGroups.Select(x => x.Path[(GroupName.Length + 2)..]).FirstOrDefault();
        }
    }

    public override void AfterMap(UserRepresentation input, UserResponse existingModel)
    {
        base.AfterMap([input], [existingModel]);
    }
}
