using Autopark.Dal.Core.Filters;
using Autopark.Dal.Core.Queries;
using Autopark.PublicApi.Shared.EventHistories.Dto;
using FluentResults;

namespace Autopark.PublicApi.Bl.EventHistories.Services.Interfaces;

public interface IEventHistoryService
{
    Task<Result<QueryResultDto<EventHistoryResponse>>> GetAllAsync(QueryFilter<EventHistoryFilterDto> filter, CancellationToken cancellationToken = default);

    Task<Result<EventHistoryResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result<EventHistoryResponse>> CreateEventHistoryAsync(
        EventHistoryRequest eventHistoryRequest,
        CancellationToken cancellationToken = default);

    Task<Result<EventHistoryResponse>> UpdateEventHistoryAsync(
        Guid id,
        EventHistoryRequest eventHistoryRequest,
        CancellationToken cancellationToken = default);

    Task<Result> DeleteEventHistoryAsync(Guid id, CancellationToken cancellationToken = default);
}
