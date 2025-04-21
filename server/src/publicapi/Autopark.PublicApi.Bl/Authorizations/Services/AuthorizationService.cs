using Autopark.Common.Attributes;
using Autopark.Common.Bl.Services.Keycloak.Interfaces;
using Autopark.Common.Constants;
using Autopark.PublicApi.Bl.Authorizations.Services.Interfaces;
using FluentResults;
using Microsoft.AspNetCore.Http;

namespace Autopark.PublicApi.Bl.Authorizations.Services;

[ServiceAsInterfaces]
public class AuthorizationService(
    IUserService userService,
    IGroupService groupService,
    IHttpContextAccessor httpContextAccessor)
    : IAuthorizationService
{
    public async Task<Result<Guid>> InitGroupAsync(string email, CancellationToken cancellationToken)
    {
        var extUser = await userService.GetUserByEmailAsync(email, cancellationToken);
        var userId = Guid.Parse(extUser.Id);
        var extGroup = await groupService.GetGroupByNameAsync(email, deepLoad: true, cancellationToken);

        if (extGroup is null)
        {
            await groupService.CreateGroupAsync(email, cancellationToken);

            extGroup = await groupService.GetGroupByNameAsync(email, deepLoad: true, cancellationToken);
        }

        var adminGroup = extGroup!.SubGroups.First(x => x.Name.EndsWith(Roles.Administrator));
        var adminGroupId = Guid.Parse(adminGroup.Id);

        var userGroups = await userService.GetUserGroupByPrefixAsync(userId, email, cancellationToken);

        if (userGroups.Any())
        {
            foreach (var group in userGroups)
            {
                await userService.RemoveUserFromGroupAsync(userId, Guid.Parse(group.Id), cancellationToken);
            }
        }

        await userService.AddUserToGroupAsync(userId, adminGroupId, cancellationToken);

        return Guid.Parse(extGroup.Id);
    }

    public async Task SelectGroupAsync(Guid groupId, CancellationToken cancellationToken)
    {
        var groupName = await groupService.GetGroupByIdAsync(groupId, cancellationToken: cancellationToken);

        var cookies = httpContextAccessor.HttpContext.Response.Cookies;
        cookies.Delete(CookieKeys.GroupId);
        cookies.Delete(CookieKeys.GroupName);
        
        cookies.Append(CookieKeys.GroupId, groupId.ToString());
        cookies.Append(CookieKeys.GroupName, groupName!.Name);
    }
}
