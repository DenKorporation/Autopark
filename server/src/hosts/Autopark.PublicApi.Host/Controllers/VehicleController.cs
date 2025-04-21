using Asp.Versioning;
using Autopark.Common.Web.Extensions;
using Autopark.Dal.Core.Filters;
using Autopark.PublicApi.Bl.Vehicles.Services.Interfaces;
using Autopark.PublicApi.Shared.Vehicles.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Autopark.PublicApi.Host.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{apiVersion:apiVersion}/[controller]/[action]")]
public class VehicleController(IVehicleService vehicleService) : ControllerBase
{
    [HttpPost]
    public async Task<IResult> PostQueryAsync(
        [FromBody] QueryFilter<VehicleFilterDto> filterRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await vehicleService.GetAllAsync(filterRequest, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpGet("{vehicleId:guid}")]
    public async Task<IResult> GetVehicleByIdAsync(Guid vehicleId, CancellationToken cancellationToken = default)
    {
        var result = await vehicleService.GetByIdAsync(vehicleId, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpPost]
    public async Task<IResult> CreateVehicleAsync(
        [FromBody] VehicleRequest createVehicleRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await vehicleService.CreateVehicleAsync(createVehicleRequest, cancellationToken);

        return result.ToAspResult(value => Results.Created(string.Empty, value));
    }

    [HttpPut("{vehicleId:guid}")]
    public async Task<IResult> UpdateVehicleAsync(
        Guid vehicleId,
        [FromBody] VehicleRequest updateVehicleRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await vehicleService.UpdateVehicleAsync(vehicleId, updateVehicleRequest, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpDelete("{vehicleId:guid}")]
    public async Task<IResult> DeleteVehicleAsync(Guid vehicleId, CancellationToken cancellationToken = default)
    {
        var result = await vehicleService.DeleteVehicleAsync(vehicleId, cancellationToken);

        return result.ToAspResult(Results.NoContent);
    }
}
