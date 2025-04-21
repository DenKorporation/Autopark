using Asp.Versioning;
using Autopark.Common.Constants;
using Autopark.Common.Web.Extensions;
using Autopark.Common.Web.Security;
using Autopark.Dal.Core.Filters;
using Autopark.PublicApi.Bl.EventTypes.Services.Interfaces;
using Autopark.PublicApi.Shared.EventTypes.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Autopark.PublicApi.Host.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{apiVersion:apiVersion}/[controller]/[action]")]
public class EventTypeController(IEventTypeService eventTypeService) : ControllerBase
{
    [HttpPost]
    public async Task<IResult> PostQueryAsync(
        [FromBody] QueryFilter<EventTypeFilterDto> filterRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await eventTypeService.GetAllAsync(filterRequest, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpGet("{eventTypeId:guid}")]
    public async Task<IResult> GetEventTypeByIdAsync(Guid eventTypeId, CancellationToken cancellationToken = default)
    {
        var result = await eventTypeService.GetByIdAsync(eventTypeId, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpPost]
    [Permission(Roles = $"{Roles.Administrator}")]
    public async Task<IResult> CreateEventTypeAsync(
        [FromBody] EventTypeRequest createEventTypeRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await eventTypeService.CreateEventTypeAsync(createEventTypeRequest, cancellationToken);

        return result.ToAspResult(value => Results.Created(string.Empty, value));
    }

    [HttpPut("{eventTypeId:guid}")]
    [Permission(Roles = $"{Roles.Administrator}")]
    public async Task<IResult> UpdateEventTypeAsync(
        Guid eventTypeId,
        [FromBody] EventTypeRequest updateEventTypeRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await eventTypeService.UpdateEventTypeAsync(eventTypeId, updateEventTypeRequest, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpDelete("{eventTypeId:guid}")]
    [Permission(Roles = $"{Roles.Administrator}")]
    public async Task<IResult> DeleteEventTypeAsync(Guid eventTypeId, CancellationToken cancellationToken = default)
    {
        var result = await eventTypeService.DeleteEventTypeAsync(eventTypeId, cancellationToken);

        return result.ToAspResult(Results.NoContent);
    }
}
