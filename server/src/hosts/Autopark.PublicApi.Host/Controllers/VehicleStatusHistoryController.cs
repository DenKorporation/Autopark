using Asp.Versioning;
using Autopark.Common.Constants;
using Autopark.Common.Web.Extensions;
using Autopark.Common.Web.Security;
using Autopark.Dal.Core.Filters;
using Autopark.PublicApi.Bl.VehicleStatusHistories.Services.Interfaces;
using Autopark.PublicApi.Shared.VehicleStatusHistories.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Autopark.PublicApi.Host.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{apiVersion:apiVersion}/[controller]/[action]")]
public class VehicleStatusHistoryController(IVehicleStatusHistoryService vehicleStatusHistoryService) : ControllerBase
{
    [HttpPost]
    public async Task<IResult> PostQueryAsync(
        [FromBody] QueryFilter<VehicleStatusHistoryFilterDto> filterRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await vehicleStatusHistoryService.GetAllAsync(filterRequest, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpGet("{vehicleStatusHistoryId:guid}")]
    public async Task<IResult> GetVehicleStatusHistoryByIdAsync(Guid vehicleStatusHistoryId, CancellationToken cancellationToken = default)
    {
        var result = await vehicleStatusHistoryService.GetByIdAsync(vehicleStatusHistoryId, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpPost]
    [Permission(Roles = $"{Roles.Administrator}, {Roles.Technician}")]
    public async Task<IResult> CreateVehicleStatusHistoryAsync(
        [FromBody] VehicleStatusHistoryRequest createVehicleStatusHistoryRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await vehicleStatusHistoryService.CreateVehicleStatusHistoryAsync(createVehicleStatusHistoryRequest, cancellationToken);

        return result.ToAspResult(value => Results.Created(string.Empty, value));
    }

    [HttpPut("{vehicleStatusHistoryId:guid}")]
    [Permission(Roles = $"{Roles.Administrator}")]
    public async Task<IResult> UpdateVehicleStatusHistoryAsync(
        Guid vehicleStatusHistoryId,
        [FromBody] VehicleStatusHistoryRequest updateVehicleStatusHistoryRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await vehicleStatusHistoryService.UpdateVehicleStatusHistoryAsync(vehicleStatusHistoryId, updateVehicleStatusHistoryRequest, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpDelete("{vehicleStatusHistoryId:guid}")]
    [Permission(Roles = $"{Roles.Administrator}")]
    public async Task<IResult> DeleteVehicleStatusHistoryAsync(Guid vehicleStatusHistoryId, CancellationToken cancellationToken = default)
    {
        var result = await vehicleStatusHistoryService.DeleteVehicleStatusHistoryAsync(vehicleStatusHistoryId, cancellationToken);

        return result.ToAspResult(Results.NoContent);
    }
}
