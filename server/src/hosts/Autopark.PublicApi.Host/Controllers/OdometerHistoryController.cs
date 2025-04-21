using Asp.Versioning;
using Autopark.Common.Constants;
using Autopark.Common.Web.Extensions;
using Autopark.Common.Web.Security;
using Autopark.Dal.Core.Filters;
using Autopark.PublicApi.Bl.OdometerHistories.Services.Interfaces;
using Autopark.PublicApi.Shared.OdometerHistories.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Autopark.PublicApi.Host.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{apiVersion:apiVersion}/[controller]/[action]")]
public class OdometerHistoryController(IOdometerHistoryService odometerHistoryService) : ControllerBase
{
    [HttpPost]
    public async Task<IResult> PostQueryAsync(
        [FromBody] QueryFilter<OdometerHistoryFilterDto> filterRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await odometerHistoryService.GetAllAsync(filterRequest, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpGet("{odometerHistoryId:guid}")]
    public async Task<IResult> GetOdometerHistoryByIdAsync(Guid odometerHistoryId, CancellationToken cancellationToken = default)
    {
        var result = await odometerHistoryService.GetByIdAsync(odometerHistoryId, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpPost]
    [Permission(Roles = $"{Roles.Administrator}, {Roles.Driver}, {Roles.Technician}")]
    public async Task<IResult> CreateOdometerHistoryAsync(
        [FromBody] OdometerHistoryRequest createOdometerHistoryRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await odometerHistoryService.CreateOdometerHistoryAsync(createOdometerHistoryRequest, cancellationToken);

        return result.ToAspResult(value => Results.Created(string.Empty, value));
    }

    [HttpPut("{odometerHistoryId:guid}")]
    [Permission(Roles = $"{Roles.Administrator}")]
    public async Task<IResult> UpdateOdometerHistoryAsync(
        Guid odometerHistoryId,
        [FromBody] OdometerHistoryRequest updateOdometerHistoryRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await odometerHistoryService.UpdateOdometerHistoryAsync(odometerHistoryId, updateOdometerHistoryRequest, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpDelete("{odometerHistoryId:guid}")]
    [Permission(Roles = $"{Roles.Administrator}")]
    public async Task<IResult> DeleteOdometerHistoryAsync(Guid odometerHistoryId, CancellationToken cancellationToken = default)
    {
        var result = await odometerHistoryService.DeleteOdometerHistoryAsync(odometerHistoryId, cancellationToken);

        return result.ToAspResult(Results.NoContent);
    }
}
