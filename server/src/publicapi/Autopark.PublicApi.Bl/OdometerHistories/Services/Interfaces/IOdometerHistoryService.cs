using Autopark.Dal.Core.Filters;
using Autopark.Dal.Core.Queries;
using Autopark.PublicApi.Shared.OdometerHistories.Dto;
using FluentResults;

namespace Autopark.PublicApi.Bl.OdometerHistories.Services.Interfaces;

public interface IOdometerHistoryService
{
    Task<Result<QueryResultDto<OdometerHistoryResponse>>> GetAllAsync(QueryFilter<OdometerHistoryFilterDto> filter, CancellationToken cancellationToken = default);

    Task<Result<OdometerHistoryResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result<OdometerHistoryResponse>> CreateOdometerHistoryAsync(
        OdometerHistoryRequest odometerHistoryRequest,
        CancellationToken cancellationToken = default);

    Task<Result<OdometerHistoryResponse>> UpdateOdometerHistoryAsync(
        Guid id,
        OdometerHistoryRequest odometerHistoryRequest,
        CancellationToken cancellationToken = default);

    Task<Result> DeleteOdometerHistoryAsync(Guid id, CancellationToken cancellationToken = default);
}
