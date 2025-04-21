using Autopark.Dal.Core.Filters;
using Autopark.Dal.Core.Queries;
using Autopark.PublicApi.Shared.VehicleStatusHistories.Dto;
using FluentResults;

namespace Autopark.PublicApi.Bl.VehicleStatusHistories.Services.Interfaces;

public interface IVehicleStatusHistoryService
{
    Task<Result<QueryResultDto<VehicleStatusHistoryResponse>>> GetAllAsync(QueryFilter<VehicleStatusHistoryFilterDto> filter, CancellationToken cancellationToken = default);

    Task<Result<VehicleStatusHistoryResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result<VehicleStatusHistoryResponse>> CreateVehicleStatusHistoryAsync(
        VehicleStatusHistoryRequest vehicleStatusHistoryRequest,
        CancellationToken cancellationToken = default);

    Task<Result<VehicleStatusHistoryResponse>> UpdateVehicleStatusHistoryAsync(
        Guid id,
        VehicleStatusHistoryRequest vehicleStatusHistoryRequest,
        CancellationToken cancellationToken = default);

    Task<Result> DeleteVehicleStatusHistoryAsync(Guid id, CancellationToken cancellationToken = default);
}
