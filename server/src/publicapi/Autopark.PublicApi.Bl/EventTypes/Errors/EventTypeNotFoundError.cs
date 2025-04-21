using Autopark.Common.Bl.Errors;

namespace Autopark.PublicApi.Bl.EventTypes.Errors;

public class EventTypeNotFoundError(string code = "EventType.NotFound", string message = "EventType not found")
    : NotFoundError(code, message)
{
    public EventTypeNotFoundError(Guid eventTypeId)
        : this(message: $"EventType {eventTypeId} not found")
    {
    }
}
