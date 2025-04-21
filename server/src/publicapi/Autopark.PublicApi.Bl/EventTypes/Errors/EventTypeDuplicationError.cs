using Autopark.Common.Bl.Errors;

namespace Autopark.PublicApi.Bl.EventTypes.Errors;

public class EventTypeDuplicationError(
    string code = "EventType.Duplication",
    string message = "EventType already exist")
    : ConflictError(code, message)
{
    public EventTypeDuplicationError(string name)
        : this(message: $"EventType '{name}' already exist")
    {
    }
}
