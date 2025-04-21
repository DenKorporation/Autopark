using Asp.Versioning;
using Autopark.Common.Constants;
using Autopark.Common.Web.Extensions;
using Autopark.Common.Web.Security;
using Autopark.Dal.Core.Filters;
using Autopark.PublicApi.Bl.Permissions.Services.Interfaces;
using Autopark.PublicApi.Shared.Permissions.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Autopark.PublicApi.Host.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{apiVersion:apiVersion}/[controller]/[action]")]
public class PermissionController(IPermissionService permissionService) : ControllerBase
{
    [HttpPost]
    public async Task<IResult> PostQueryAsync(
        [FromBody] QueryFilter<PermissionFilterDto> filterRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await permissionService.GetAllAsync(filterRequest, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpGet("{permissionId:guid}")]
    public async Task<IResult> GetPermissionByIdAsync(Guid permissionId, CancellationToken cancellationToken = default)
    {
        var result = await permissionService.GetByIdAsync(permissionId, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpPost]
    [Permission(Roles = $"{Roles.Administrator}, {Roles.DocumentSpecialist}")]
    public async Task<IResult> CreatePermissionAsync(
        [FromBody] PermissionRequest createPermissionRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await permissionService.CreatePermissionAsync(createPermissionRequest, cancellationToken);

        return result.ToAspResult(value => Results.Created(string.Empty, value));
    }

    [HttpPut("{permissionId:guid}")]
    [Permission(Roles = $"{Roles.Administrator}, {Roles.DocumentSpecialist}")]
    public async Task<IResult> UpdatePermissionAsync(
        Guid permissionId,
        [FromBody] PermissionRequest updatePermissionRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await permissionService.UpdatePermissionAsync(permissionId, updatePermissionRequest, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpDelete("{permissionId:guid}")]
    [Permission(Roles = $"{Roles.Administrator}, {Roles.DocumentSpecialist}")]
    public async Task<IResult> DeletePermissionAsync(Guid permissionId, CancellationToken cancellationToken = default)
    {
        var result = await permissionService.DeletePermissionAsync(permissionId, cancellationToken);

        return result.ToAspResult(Results.NoContent);
    }
}
