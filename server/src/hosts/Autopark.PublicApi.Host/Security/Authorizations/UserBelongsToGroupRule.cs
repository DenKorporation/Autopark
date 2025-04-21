using Autopark.Common.Bl.Services.Keycloak.Interfaces;
using Autopark.Common.Security;
using Autopark.Common.Web;
using Autopark.Common.Web.Security;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Autopark.PublicApi.Host.Security.Authorizations;

public class UserBelongsToGroupRule(
    IUserInfoProvider userInfoProvider,
    IUserService userService
    ) : IPermissionRule
{
    public async Task<bool> AllowedAsync(ActionExecutingContext context)
    {
        var groupId = context.GetActionArgument<Guid>("groupId");

        var userId = userInfoProvider.GetUserId();

        if (!userId.HasValue)
        {
            return false;
        }

        var userGroups = await userService.GetUserGroupsAsync(userId.Value);

        var groupIds = userGroups.Select(x => x.ParentId ?? x.Id).ToList();

        return groupIds.Any(x => x == groupId.ToString());
    }
}
