using Asp.Versioning;
using Autopark.Common.Security;
using Autopark.Common.Web.Extensions;
using Autopark.PublicApi.Bl.Groups.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Autopark.PublicApi.Host.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{apiVersion:apiVersion}/[controller]/[action]")]
public class GroupController(
    IGroupsService groupsService,
    IUserInfoProvider userInfoProvider)
    : ControllerBase
{
    [HttpGet]
    public async Task<IResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var userId = userInfoProvider.GetUserId();

        var result = await groupsService.GetAllUserGroupsAsync(userId!.Value, cancellationToken);
        
        return result.ToAspResult(Results.Ok);
    }
}
