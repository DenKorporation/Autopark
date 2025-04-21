using Autopark.Common.Attributes;
using Autopark.Common.Bl.Services.Keycloak.Interfaces;
using Autopark.Common.Mapping;
using Autopark.Common.Security;
using Autopark.Dal.Core.Filters;
using Autopark.Dal.Core.Queries;
using Autopark.PublicApi.Bl.Users.Errors;
using Autopark.PublicApi.Shared.Users.Dto;
using FluentResults;
using FS.Keycloak.RestApiClient.Model;
using IKeycloakUserService = Autopark.Common.Bl.Services.Keycloak.Interfaces.IUserService;
using IUserService = Autopark.PublicApi.Bl.Users.Services.Interfaces.IUserService;

namespace Autopark.PublicApi.Bl.Users.Services;

[ServiceAsInterfaces]
public class UserService(
    IExtendedMapper<UserRepresentation, UserResponse> userMapper,
    IMapper<UserRequest, UserRepresentation> userRequestMapper,
    IUserInfoProvider userInfoProvider,
    IGroupService groupService,
    IKeycloakUserService userService)
    : IUserService
{
    public async Task<Result<QueryResultDto<UserResponse>>> GetAllAsync(QueryFilter<UserFilterDto> filter, CancellationToken cancellationToken = default)
    {
        var groupId = userInfoProvider.GetGroupId();
        
        var users = await groupService.GetGroupMembersAsync(groupId, cancellationToken);

        users = ApplyFilter(users, filter.Filter);

        return ConvertToQueryResult(users, filter);
    }

    public async Task<Result<UserResponse>> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await userService.GetUserByEmailAsync(email, cancellationToken);

        if (user is null)
        {
            return new UserNotFoundError();
        }

        var result = new UserResponse();
        userMapper.Map(user, result, useAfterMap: true);

        return result;
    }

    public async Task<Result<UserResponse>> CreateUserAsync(UserRequest userRequest, CancellationToken cancellationToken = default)
    {
        var existingUser = await userService.GetUserByEmailAsync(userRequest.Email, cancellationToken);

        if (existingUser is not null)
        {
            return new UserDuplicationError(userRequest.Email);
        }

        var groupName = userInfoProvider.GetGroupName();
        var request = userRequestMapper.Map(userRequest);

        request.Enabled = true;
        request.EmailVerified = true;
        request.Credentials = [
            new CredentialRepresentation()
            {
                Temporary = false,
                Type = "password",
                Value = userRequest.Password
            }];

        request.Groups = [$"/{groupName}/{userRequest.Role}"];

        await userService.CreateUserAsync(request, cancellationToken);

        return await GetByEmailAsync(userRequest.Email, cancellationToken);
    }

    private IList<UserRepresentation> ApplyFilter(IList<UserRepresentation> users, UserFilterDto filter)
    {
        IList<UserRepresentation> result = users;

        if (filter.Email is not null)
        {
            result = users.Where(x => x.Email.Contains(filter.Email)).ToList();
        }
        
        if (filter.FirstName is not null)
        {
            result = users.Where(x => x.FirstName.Contains(filter.FirstName)).ToList();
        }
        
        if (filter.LastName is not null)
        {
            result = users.Where(x => x.FirstName.Contains(filter.LastName)).ToList();
        }

        return result;
    }

    private QueryResultDto<UserResponse> ConvertToQueryResult(IList<UserRepresentation> users, QueryFilterBase filter)
    {
        var result = new QueryResultDto<UserResponse>();

        if (filter.OnlyCount)
        {
            result.Count = users.Count;

            return result;
        }

        if (filter.WithCount == true)
        {
            result.Count = users.Count;
        }

        if (filter.Skip is not null)
        {
            users = users.Skip(filter.Skip.Value).ToList();
        }

        if (filter.Take is not null)
        {
            users = users.Take(filter.Take.Value).ToList();
        }

        result.Items = userMapper.Project(users.AsQueryable(), enableDeepMapping: true);

        return result;
    }
}
