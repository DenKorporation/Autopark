using Asp.Versioning;
using Autopark.Common.Constants;
using Autopark.Common.Web.Extensions;
using Autopark.Common.Web.Security;
using Autopark.Dal.Core.Filters;
using Autopark.PublicApi.Bl.MaintenanceRecords.Services.Interfaces;
using Autopark.PublicApi.Shared.MaintenanceRecords.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Autopark.PublicApi.Host.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{apiVersion:apiVersion}/[controller]/[action]")]
public class MaintenanceRecordController(IMaintenanceRecordService maintenanceRecordService) : ControllerBase
{
    [HttpPost]
    public async Task<IResult> PostQueryAsync(
        [FromBody] QueryFilter<MaintenanceRecordFilterDto> filterRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await maintenanceRecordService.GetAllAsync(filterRequest, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpGet("{maintenanceRecordId:guid}")]
    public async Task<IResult> GetMaintenanceRecordByIdAsync(Guid maintenanceRecordId, CancellationToken cancellationToken = default)
    {
        var result = await maintenanceRecordService.GetByIdAsync(maintenanceRecordId, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpPost]
    [Permission(Roles = $"{Roles.Administrator}, {Roles.Technician}")]
    public async Task<IResult> CreateMaintenanceRecordAsync(
        [FromBody] MaintenanceRecordRequest createMaintenanceRecordRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await maintenanceRecordService.CreateMaintenanceRecordAsync(createMaintenanceRecordRequest, cancellationToken);

        return result.ToAspResult(value => Results.Created(string.Empty, value));
    }

    [HttpPut("{maintenanceRecordId:guid}")]
    [Permission(Roles = $"{Roles.Administrator}, {Roles.Technician}")]
    public async Task<IResult> UpdateMaintenanceRecordAsync(
        Guid maintenanceRecordId,
        [FromBody] MaintenanceRecordRequest updateMaintenanceRecordRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await maintenanceRecordService.UpdateMaintenanceRecordAsync(maintenanceRecordId, updateMaintenanceRecordRequest, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpDelete("{maintenanceRecordId:guid}")]
    [Permission(Roles = $"{Roles.Administrator}, {Roles.Technician}")]
    public async Task<IResult> DeleteMaintenanceRecordAsync(Guid maintenanceRecordId, CancellationToken cancellationToken = default)
    {
        var result = await maintenanceRecordService.DeleteMaintenanceRecordAsync(maintenanceRecordId, cancellationToken);

        return result.ToAspResult(Results.NoContent);
    }
}
