using Asp.Versioning;
using Autopark.Common.Constants;
using Autopark.Common.Web.Extensions;
using Autopark.Common.Web.Security;
using Autopark.Dal.Core.Filters;
using Autopark.PublicApi.Bl.Statuses.Services.Interfaces;
using Autopark.PublicApi.Shared.Statuses.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Autopark.PublicApi.Host.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{apiVersion:apiVersion}/[controller]/[action]")]
public class StatusController(IStatusService statusService) : ControllerBase
{
    [HttpPost]
    public async Task<IResult> PostQueryAsync(
        [FromBody] QueryFilter<StatusFilterDto> filterRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await statusService.GetAllAsync(filterRequest, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpGet("{statusId:guid}")]
    public async Task<IResult> GetStatusByIdAsync(Guid statusId, CancellationToken cancellationToken = default)
    {
        var result = await statusService.GetByIdAsync(statusId, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpPost]
    [Permission(Roles = $"{Roles.Administrator}")]
    public async Task<IResult> CreateStatusAsync(
        [FromBody] StatusRequest createStatusRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await statusService.CreateStatusAsync(createStatusRequest, cancellationToken);

        return result.ToAspResult(value => Results.Created(string.Empty, value));
    }

    [HttpPut("{statusId:guid}")]
    [Permission(Roles = $"{Roles.Administrator}")]
    public async Task<IResult> UpdateStatusAsync(
        Guid statusId,
        [FromBody] StatusRequest updateStatusRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await statusService.UpdateStatusAsync(statusId, updateStatusRequest, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpDelete("{statusId:guid}")]
    [Permission(Roles = $"{Roles.Administrator}")]
    public async Task<IResult> DeleteStatusAsync(Guid statusId, CancellationToken cancellationToken = default)
    {
        var result = await statusService.DeleteStatusAsync(statusId, cancellationToken);

        return result.ToAspResult(Results.NoContent);
    }
}
