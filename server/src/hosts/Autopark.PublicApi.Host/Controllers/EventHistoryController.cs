using Asp.Versioning;
using Autopark.Common.Constants;
using Autopark.Common.Web.Extensions;
using Autopark.Common.Web.Security;
using Autopark.Dal.Core.Filters;
using Autopark.PublicApi.Bl.EventHistories.Services.Interfaces;
using Autopark.PublicApi.Shared.EventHistories.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Autopark.PublicApi.Host.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{apiVersion:apiVersion}/[controller]/[action]")]
public class EventHistoryController(IEventHistoryService eventHistoryService) : ControllerBase
{
    [HttpPost]
    public async Task<IResult> PostQueryAsync(
        [FromBody] QueryFilter<EventHistoryFilterDto> filterRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await eventHistoryService.GetAllAsync(filterRequest, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpGet("{eventHistoryId:guid}")]
    public async Task<IResult> GetEventHistoryByIdAsync(Guid eventHistoryId, CancellationToken cancellationToken = default)
    {
        var result = await eventHistoryService.GetByIdAsync(eventHistoryId, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpPost]
    public async Task<IResult> CreateEventHistoryAsync(
        [FromBody] EventHistoryRequest createEventHistoryRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await eventHistoryService.CreateEventHistoryAsync(createEventHistoryRequest, cancellationToken);

        return result.ToAspResult(value => Results.Created(string.Empty, value));
    }

    [HttpPut("{eventHistoryId:guid}")]
    [Permission(Roles = $"{Roles.Administrator}")]
    public async Task<IResult> UpdateEventHistoryAsync(
        Guid eventHistoryId,
        [FromBody] EventHistoryRequest updateEventHistoryRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await eventHistoryService.UpdateEventHistoryAsync(eventHistoryId, updateEventHistoryRequest, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpDelete("{eventHistoryId:guid}")]
    [Permission(Roles = $"{Roles.Administrator}")]
    public async Task<IResult> DeleteEventHistoryAsync(Guid eventHistoryId, CancellationToken cancellationToken = default)
    {
        var result = await eventHistoryService.DeleteEventHistoryAsync(eventHistoryId, cancellationToken);

        return result.ToAspResult(Results.NoContent);
    }
}
