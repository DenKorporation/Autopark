using Asp.Versioning;
using Autopark.Common.Constants;
using Autopark.Common.Web.Extensions;
using Autopark.Common.Web.Security;
using Autopark.Dal.Core.Filters;
using Autopark.PublicApi.Bl.Users.Services.Interfaces;
using Autopark.PublicApi.Shared.Users.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Autopark.PublicApi.Host.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{apiVersion:apiVersion}/[controller]/[action]")]
public class UserController(
    IUserService userService)
    : ControllerBase
{
    [HttpGet("")]
    public async Task<IResult> GetAllUsersAsync(
        [FromBody] QueryFilter<UserFilterDto> filterRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await userService.GetAllAsync(filterRequest, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpGet("{filterEmail}")]
    [Permission(Roles = $"{Roles.Administrator}")]
    public async Task<IResult> GetUserByEmailAsync(string filterEmail, CancellationToken cancellationToken = default)
    {
        var result = await userService.GetByEmailAsync(filterEmail, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpPost]
    [Permission(Roles = $"{Roles.Administrator}")]
    public async Task<IResult> CreateUserAsync(
        [FromBody] UserRequest createUserRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await userService.CreateUserAsync(createUserRequest, cancellationToken);

        return result.ToAspResult(value => Results.Created(string.Empty, value));
    }
}
