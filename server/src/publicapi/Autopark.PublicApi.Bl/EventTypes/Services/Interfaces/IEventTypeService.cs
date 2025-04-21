using Autopark.Dal.Core.Filters;
using Autopark.Dal.Core.Queries;
using Autopark.PublicApi.Shared.EventTypes.Dto;
using FluentResults;

namespace Autopark.PublicApi.Bl.EventTypes.Services.Interfaces;

public interface IEventTypeService
{
    Task<Result<QueryResultDto<EventTypeResponse>>> GetAllAsync(QueryFilter<EventTypeFilterDto> filter, CancellationToken cancellationToken = default);

    Task<Result<EventTypeResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result<EventTypeResponse>> CreateEventTypeAsync(
        EventTypeRequest eventTypeRequest,
        CancellationToken cancellationToken = default);

    Task<Result<EventTypeResponse>> UpdateEventTypeAsync(
        Guid id,
        EventTypeRequest eventTypeRequest,
        CancellationToken cancellationToken = default);

    Task<Result> DeleteEventTypeAsync(Guid id, CancellationToken cancellationToken = default);
}
