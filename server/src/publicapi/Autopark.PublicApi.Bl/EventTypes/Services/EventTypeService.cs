using Autopark.Common.Attributes;
using Autopark.Common.Bl.Errors;
using Autopark.Dal.Core.Filters;
using Autopark.Dal.Core.Queries;
using Autopark.Dal.Core.Repositories;
using Autopark.PublicApi.Bl.EventTypes.Errors;
using Autopark.PublicApi.Bl.EventTypes.Services.Interfaces;
using Autopark.PublicApi.Models.EventTypes;
using Autopark.PublicApi.Shared.EventTypes.Dto;
using FluentResults;

namespace Autopark.PublicApi.Bl.EventTypes.Services;

[ServiceAsInterfaces]
public class EventTypeService(
    IEditDtoRepository<EventType, EventTypeRequest> eventTypeEditDtoRepository,
    IDtoRepository<EventType, EventTypeResponse> eventTypeDtoRepository) : IEventTypeService
{
    public async Task<Result<QueryResultDto<EventTypeResponse>>> GetAllAsync(QueryFilter<EventTypeFilterDto> filter, CancellationToken cancellationToken = default)
    {
        return await eventTypeDtoRepository.QueryDtoAsync(filter, cancellationToken);
    }

    public async Task<Result<EventTypeResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var eventTypeDto = await eventTypeDtoRepository.GetDtoAsync(id, cancellationToken);

        if (eventTypeDto is null)
        {
            return new EventTypeNotFoundError(id);
        }

        return eventTypeDto;
    }

    public async Task<Result<EventTypeResponse>> CreateEventTypeAsync(EventTypeRequest eventTypeRequest, CancellationToken cancellationToken = default)
    {
        var result = await EnsureEventTypeByNameNotExistAsync(eventTypeRequest.Name, cancellationToken);
        if (result.IsFailed)
        {
            return result;
        }

        try
        {
            var id = await eventTypeEditDtoRepository.SaveDtoAsync(eventTypeRequest, cancellationToken: cancellationToken);

            return (await eventTypeDtoRepository.GetDtoAsync(id, cancellationToken))!;
        }
        catch (Exception)
        {
            return new InternalServerError("EventType.Create");
        }
    }

    public async Task<Result<EventTypeResponse>> UpdateEventTypeAsync(Guid id, EventTypeRequest eventTypeRequest, CancellationToken cancellationToken = default)
    {
        var eventTypeDto = await eventTypeDtoRepository.GetDtoAsync(id, cancellationToken);
        if (eventTypeDto is null)
        {
            return new EventTypeNotFoundError(id);
        }

        if (eventTypeDto.Name != eventTypeRequest.Name)
        {
            var result = await EnsureEventTypeByNameNotExistAsync(eventTypeRequest.Name, cancellationToken);
            if (result.IsFailed)
            {
                return result;
            }    
        }

        try
        {
            var updatedId = await eventTypeEditDtoRepository.SaveDtoAsync(eventTypeRequest, cancellationToken: cancellationToken);

            return (await eventTypeDtoRepository.GetDtoAsync(id, cancellationToken))!;
        }
        catch (Exception)
        {
            return new InternalServerError("EventType.Update");
        }
    }

    public async Task<Result> DeleteEventTypeAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var eventType = await eventTypeDtoRepository.GetDtoAsync(id, cancellationToken);

        if (eventType is null)
        {
            return new EventTypeNotFoundError(id);
        }

        try
        {
            await eventTypeDtoRepository.DeleteByIdAsync(id);
        }
        catch (Exception)
        {
            return new InternalServerError("EventType.Delete");
        }

        return Result.Ok();
    }

    private async Task<Result> EnsureEventTypeByNameNotExistAsync(string name, CancellationToken cancellationToken = default)
    {
        var eventTypeExist = await eventTypeEditDtoRepository.AnyByFilterAsync(
            new EventTypeFilterDto()
            {
                Name = name,
            }, cancellationToken);

        if (eventTypeExist)
        {
            return new EventTypeDuplicationError(name);
        }

        return Result.Ok();
    }
}
