using Autopark.Common.Bl.Errors;

namespace Autopark.PublicApi.Bl.Statuses.Errors;

public class StatusDuplicationError(
    string code = "Status.Duplication",
    string message = "Status already exist")
    : ConflictError(code, message)
{
    public StatusDuplicationError(string name)
        : this(message: $"Status '{name}' already exist")
    {
    }
}
