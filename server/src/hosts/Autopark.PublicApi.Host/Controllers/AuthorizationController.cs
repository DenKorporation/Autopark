using Asp.Versioning;
using Autopark.Common.Constants;
using Autopark.Common.Security;
using Autopark.Common.Web.Extensions;
using Autopark.Common.Web.Security;
using Autopark.PublicApi.Bl.Authorizations.Services.Interfaces;
using Autopark.PublicApi.Host.Security.Authorizations;
using Microsoft.AspNetCore.Mvc;

namespace Autopark.PublicApi.Host.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{apiVersion:apiVersion}/[controller]/[action]")]
public class AuthorizationController(
    IAuthorizationService authorizationService,
    IUserInfoProvider userInfoProvider) : ControllerBase
{
    [HttpPost]
    public async Task<IResult> InitializeGroupAsync(CancellationToken cancellationToken)
    {
        var groupName = userInfoProvider.GetCurrentUserLogin();

        var result = await authorizationService.InitGroupAsync(groupName, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }
    
    [HttpPost]
    [Permission(typeof(UserBelongsToGroupRule))]
    public async Task SelectGroupAsync(Guid groupId, CancellationToken cancellationToken)
    {
        await authorizationService.SelectGroupAsync(groupId, cancellationToken);
    }

    [HttpPost]
    public void Logout()
    {
        var cookies = HttpContext.Response.Cookies;
        cookies.Delete(CookieKeys.GroupId);
        cookies.Delete(CookieKeys.GroupName);
    }
}
