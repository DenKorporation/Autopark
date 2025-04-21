using Autopark.Common.Attributes;
using Autopark.Common.Bl.Errors;
using Autopark.Dal.Core.Filters;
using Autopark.Dal.Core.Queries;
using Autopark.Dal.Core.Repositories;
using Autopark.PublicApi.Bl.EventHistories.Errors;
using Autopark.PublicApi.Bl.EventHistories.Services.Interfaces;
using Autopark.PublicApi.Models.EventHistories;
using Autopark.PublicApi.Shared.EventHistories.Dto;
using FluentResults;

namespace Autopark.PublicApi.Bl.EventHistories.Services;

[ServiceAsInterfaces]
public class EventHistoryService(
    IEditDtoRepository<EventHistory, EventHistoryRequest> eventHistoryEditDtoRepository,
    IDtoRepository<EventHistory, EventHistoryResponse> eventHistoryDtoRepository) : IEventHistoryService
{
    public async Task<Result<QueryResultDto<EventHistoryResponse>>> GetAllAsync(QueryFilter<EventHistoryFilterDto> filter, CancellationToken cancellationToken = default)
    {
        return await eventHistoryDtoRepository.QueryDtoAsync(filter, cancellationToken);
    }

    public async Task<Result<EventHistoryResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var eventHistoryDto = await eventHistoryDtoRepository.GetDtoAsync(id, cancellationToken);

        if (eventHistoryDto is null)
        {
            return new EventHistoryNotFoundError(id);
        }

        return eventHistoryDto;
    }

    public async Task<Result<EventHistoryResponse>> CreateEventHistoryAsync(EventHistoryRequest eventHistoryRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            var id = await eventHistoryEditDtoRepository.SaveDtoAsync(eventHistoryRequest, cancellationToken: cancellationToken);

            return (await eventHistoryDtoRepository.GetDtoAsync(id, cancellationToken))!;
        }
        catch (Exception)
        {
            return new InternalServerError("EventHistory.Create");
        }
    }

    public async Task<Result<EventHistoryResponse>> UpdateEventHistoryAsync(Guid id, EventHistoryRequest eventHistoryRequest, CancellationToken cancellationToken = default)
    {
        var eventHistoryDto = await eventHistoryDtoRepository.GetDtoAsync(id, cancellationToken);
        if (eventHistoryDto is null)
        {
            return new EventHistoryNotFoundError(id);
        }

        try
        {
            var updatedId = await eventHistoryEditDtoRepository.SaveDtoAsync(eventHistoryRequest, cancellationToken: cancellationToken);

            return (await eventHistoryDtoRepository.GetDtoAsync(id, cancellationToken))!;
        }
        catch (Exception)
        {
            return new InternalServerError("EventHistory.Update");
        }
    }

    public async Task<Result> DeleteEventHistoryAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var eventHistory = await eventHistoryDtoRepository.GetDtoAsync(id, cancellationToken);

        if (eventHistory is null)
        {
            return new EventHistoryNotFoundError(id);
        }

        try
        {
            await eventHistoryDtoRepository.DeleteByIdAsync(id);
        }
        catch (Exception)
        {
            return new InternalServerError("EventHistory.Delete");
        }

        return Result.Ok();
    }
}
