using Asp.Versioning;
using Autopark.Common.Constants;
using Autopark.Common.Web.Extensions;
using Autopark.Common.Web.Security;
using Autopark.Dal.Core.Filters;
using Autopark.PublicApi.Bl.RefuelingHistories.Services.Interfaces;
using Autopark.PublicApi.Shared.RefuelingHistories.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Autopark.PublicApi.Host.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{apiVersion:apiVersion}/[controller]/[action]")]
public class RefuelingHistoryController(IRefuelingHistoryService refuelingHistoryService) : ControllerBase
{
    [HttpPost]
    public async Task<IResult> PostQueryAsync(
        [FromBody] QueryFilter<RefuelingHistoryFilterDto> filterRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await refuelingHistoryService.GetAllAsync(filterRequest, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpGet("{refuelingHistoryId:guid}")]
    public async Task<IResult> GetRefuelingHistoryByIdAsync(Guid refuelingHistoryId, CancellationToken cancellationToken = default)
    {
        var result = await refuelingHistoryService.GetByIdAsync(refuelingHistoryId, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpPost]
    [Permission(Roles = $"{Roles.Administrator}, {Roles.Driver}, {Roles.Technician}")]
    public async Task<IResult> CreateRefuelingHistoryAsync(
        [FromBody] RefuelingHistoryRequest createRefuelingHistoryRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await refuelingHistoryService.CreateRefuelingHistoryAsync(createRefuelingHistoryRequest, cancellationToken);

        return result.ToAspResult(value => Results.Created(string.Empty, value));
    }

    [HttpPut("{refuelingHistoryId:guid}")]
    [Permission(Roles = $"{Roles.Administrator}")]
    public async Task<IResult> UpdateRefuelingHistoryAsync(
        Guid refuelingHistoryId,
        [FromBody] RefuelingHistoryRequest updateRefuelingHistoryRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await refuelingHistoryService.UpdateRefuelingHistoryAsync(refuelingHistoryId, updateRefuelingHistoryRequest, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpDelete("{refuelingHistoryId:guid}")]
    [Permission(Roles = $"{Roles.Administrator}")]
    public async Task<IResult> DeleteRefuelingHistoryAsync(Guid refuelingHistoryId, CancellationToken cancellationToken = default)
    {
        var result = await refuelingHistoryService.DeleteRefuelingHistoryAsync(refuelingHistoryId, cancellationToken);

        return result.ToAspResult(Results.NoContent);
    }
}
