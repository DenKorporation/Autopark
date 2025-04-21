using Autopark.Common.Bl.Errors;

namespace Autopark.PublicApi.Bl.EventHistories.Errors;

public class EventHistoryNotFoundError(string code = "EventHistory.NotFound", string message = "EventHistory not found")
    : NotFoundError(code, message)
{
    public EventHistoryNotFoundError(Guid eventHistoryId)
        : this(message: $"EventHistory {eventHistoryId} not found")
    {
    }
}
