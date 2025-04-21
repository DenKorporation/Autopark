using Asp.Versioning;
using Autopark.Common.Constants;
using Autopark.Common.Web.Extensions;
using Autopark.Common.Web.Security;
using Autopark.Dal.Core.Filters;
using Autopark.PublicApi.Bl.FuelTypes.Services.Interfaces;
using Autopark.PublicApi.Shared.FuelTypes.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Autopark.PublicApi.Host.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{apiVersion:apiVersion}/[controller]/[action]")]
public class FuelTypeController(IFuelTypeService fuelTypeService) : ControllerBase
{
    [HttpPost]
    public async Task<IResult> PostQueryAsync(
        [FromBody] QueryFilter<FuelTypeFilterDto> filterRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await fuelTypeService.GetAllAsync(filterRequest, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpGet("{fuelTypeId:guid}")]
    public async Task<IResult> GetFuelTypeByIdAsync(Guid fuelTypeId, CancellationToken cancellationToken = default)
    {
        var result = await fuelTypeService.GetByIdAsync(fuelTypeId, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpPost]
    [Permission(Roles = $"{Roles.Administrator}")]
    public async Task<IResult> CreateFuelTypeAsync(
        [FromBody] FuelTypeRequest createFuelTypeRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await fuelTypeService.CreateFuelTypeAsync(createFuelTypeRequest, cancellationToken);

        return result.ToAspResult(value => Results.Created(string.Empty, value));
    }

    [HttpPut("{fuelTypeId:guid}")]
    [Permission(Roles = $"{Roles.Administrator}")]
    public async Task<IResult> UpdateFuelTypeAsync(
        Guid fuelTypeId,
        [FromBody] FuelTypeRequest updateFuelTypeRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await fuelTypeService.UpdateFuelTypeAsync(fuelTypeId, updateFuelTypeRequest, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpDelete("{fuelTypeId:guid}")]
    [Permission(Roles = $"{Roles.Administrator}")]
    public async Task<IResult> DeleteFuelTypeAsync(Guid fuelTypeId, CancellationToken cancellationToken = default)
    {
        var result = await fuelTypeService.DeleteFuelTypeAsync(fuelTypeId, cancellationToken);

        return result.ToAspResult(Results.NoContent);
    }
}
