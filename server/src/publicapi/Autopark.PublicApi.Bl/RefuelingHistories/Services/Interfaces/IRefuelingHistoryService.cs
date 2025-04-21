using Autopark.Dal.Core.Filters;
using Autopark.Dal.Core.Queries;
using Autopark.PublicApi.Shared.RefuelingHistories.Dto;
using FluentResults;

namespace Autopark.PublicApi.Bl.RefuelingHistories.Services.Interfaces;

public interface IRefuelingHistoryService
{
    Task<Result<QueryResultDto<RefuelingHistoryResponse>>> GetAllAsync(QueryFilter<RefuelingHistoryFilterDto> filter, CancellationToken cancellationToken = default);

    Task<Result<RefuelingHistoryResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result<RefuelingHistoryResponse>> CreateRefuelingHistoryAsync(
        RefuelingHistoryRequest refuelingHistoryRequest,
        CancellationToken cancellationToken = default);

    Task<Result<RefuelingHistoryResponse>> UpdateRefuelingHistoryAsync(
        Guid id,
        RefuelingHistoryRequest refuelingHistoryRequest,
        CancellationToken cancellationToken = default);

    Task<Result> DeleteRefuelingHistoryAsync(Guid id, CancellationToken cancellationToken = default);
}
