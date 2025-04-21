using Autopark.Dal.Core.Filters;
using Autopark.Dal.Core.Queries;
using Autopark.PublicApi.Shared.MaintenanceRecords.Dto;
using FluentResults;

namespace Autopark.PublicApi.Bl.MaintenanceRecords.Services.Interfaces;

public interface IMaintenanceRecordService
{
    Task<Result<QueryResultDto<MaintenanceRecordResponse>>> GetAllAsync(QueryFilter<MaintenanceRecordFilterDto> filter, CancellationToken cancellationToken = default);

    Task<Result<MaintenanceRecordResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result<MaintenanceRecordResponse>> CreateMaintenanceRecordAsync(
        MaintenanceRecordRequest maintenanceRecordRequest,
        CancellationToken cancellationToken = default);

    Task<Result<MaintenanceRecordResponse>> UpdateMaintenanceRecordAsync(
        Guid id,
        MaintenanceRecordRequest maintenanceRecordRequest,
        CancellationToken cancellationToken = default);

    Task<Result> DeleteMaintenanceRecordAsync(Guid id, CancellationToken cancellationToken = default);
}
