using Autopark.Common.Attributes;
using Autopark.Common.Bl.Errors;
using Autopark.Dal.Core.Filters;
using Autopark.Dal.Core.Queries;
using Autopark.Dal.Core.Repositories;
using Autopark.PublicApi.Bl.MaintenanceRecords.Errors;
using Autopark.PublicApi.Bl.MaintenanceRecords.Services.Interfaces;
using Autopark.PublicApi.Models.MaintenanceRecords;
using Autopark.PublicApi.Shared.MaintenanceRecords.Dto;
using FluentResults;

namespace Autopark.PublicApi.Bl.MaintenanceRecords.Services;

[ServiceAsInterfaces]
public class MaintenanceRecordService(
    IEditDtoRepository<MaintenanceRecord, MaintenanceRecordRequest> maintenanceRecordEditDtoRepository,
    IDtoRepository<MaintenanceRecord, MaintenanceRecordResponse> maintenanceRecordDtoRepository) : IMaintenanceRecordService
{
    public async Task<Result<QueryResultDto<MaintenanceRecordResponse>>> GetAllAsync(QueryFilter<MaintenanceRecordFilterDto> filter, CancellationToken cancellationToken = default)
    {
        return await maintenanceRecordDtoRepository.QueryDtoAsync(filter, cancellationToken);
    }

    public async Task<Result<MaintenanceRecordResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var maintenanceRecordDto = await maintenanceRecordDtoRepository.GetDtoAsync(id, cancellationToken);

        if (maintenanceRecordDto is null)
        {
            return new MaintenanceRecordNotFoundError(id);
        }

        return maintenanceRecordDto;
    }

    public async Task<Result<MaintenanceRecordResponse>> CreateMaintenanceRecordAsync(MaintenanceRecordRequest maintenanceRecordRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            var id = await maintenanceRecordEditDtoRepository.SaveDtoAsync(maintenanceRecordRequest, cancellationToken: cancellationToken);

            return (await maintenanceRecordDtoRepository.GetDtoAsync(id, cancellationToken))!;
        }
        catch (Exception)
        {
            return new InternalServerError("MaintenanceRecord.Create");
        }
    }

    public async Task<Result<MaintenanceRecordResponse>> UpdateMaintenanceRecordAsync(Guid id, MaintenanceRecordRequest maintenanceRecordRequest, CancellationToken cancellationToken = default)
    {
        var maintenanceRecordDto = await maintenanceRecordDtoRepository.GetDtoAsync(id, cancellationToken);
        if (maintenanceRecordDto is null)
        {
            return new MaintenanceRecordNotFoundError(id);
        }

        try
        {
            var updatedId = await maintenanceRecordEditDtoRepository.SaveDtoAsync(maintenanceRecordRequest, cancellationToken: cancellationToken);

            return (await maintenanceRecordDtoRepository.GetDtoAsync(id, cancellationToken))!;
        }
        catch (Exception)
        {
            return new InternalServerError("MaintenanceRecord.Update");
        }
    }

    public async Task<Result> DeleteMaintenanceRecordAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var maintenanceRecord = await maintenanceRecordDtoRepository.GetDtoAsync(id, cancellationToken);

        if (maintenanceRecord is null)
        {
            return new MaintenanceRecordNotFoundError(id);
        }

        try
        {
            await maintenanceRecordDtoRepository.DeleteByIdAsync(id);
        }
        catch (Exception)
        {
            return new InternalServerError("MaintenanceRecord.Delete");
        }

        return Result.Ok();
    }
}
