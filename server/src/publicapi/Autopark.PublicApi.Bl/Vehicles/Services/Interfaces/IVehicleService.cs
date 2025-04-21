using Autopark.Dal.Core.Filters;
using Autopark.Dal.Core.Queries;
using Autopark.PublicApi.Shared.Vehicles.Dto;
using FluentResults;

namespace Autopark.PublicApi.Bl.Vehicles.Services.Interfaces;

public interface IVehicleService
{
    Task<Result<QueryResultDto<VehicleResponse>>> GetAllAsync(QueryFilter<VehicleFilterDto> filter, CancellationToken cancellationToken = default);

    Task<Result<VehicleResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result<VehicleResponse>> CreateVehicleAsync(
        VehicleRequest vehicleRequest,
        CancellationToken cancellationToken = default);

    Task<Result<VehicleResponse>> UpdateVehicleAsync(
        Guid id,
        VehicleRequest vehicleRequest,
        CancellationToken cancellationToken = default);

    Task<Result> DeleteVehicleAsync(Guid id, CancellationToken cancellationToken = default);
}
