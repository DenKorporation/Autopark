using Autopark.Dal.Core.Filters;
using Autopark.Dal.Core.Queries;
using Autopark.PublicApi.Shared.FuelTypes.Dto;
using FluentResults;

namespace Autopark.PublicApi.Bl.FuelTypes.Services.Interfaces;

public interface IFuelTypeService
{
    Task<Result<QueryResultDto<FuelTypeResponse>>> GetAllAsync(QueryFilter<FuelTypeFilterDto> filter, CancellationToken cancellationToken = default);

    Task<Result<FuelTypeResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result<FuelTypeResponse>> CreateFuelTypeAsync(
        FuelTypeRequest fuelTypeRequest,
        CancellationToken cancellationToken = default);

    Task<Result<FuelTypeResponse>> UpdateFuelTypeAsync(
        Guid id,
        FuelTypeRequest fuelTypeRequest,
        CancellationToken cancellationToken = default);

    Task<Result> DeleteFuelTypeAsync(Guid id, CancellationToken cancellationToken = default);
}
