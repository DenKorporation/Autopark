using Autopark.Common.Bl.Errors;

namespace Autopark.PublicApi.Bl.Statuses.Errors;

public class StatusNotFoundError(string code = "Status.NotFound", string message = "Status not found")
    : NotFoundError(code, message)
{
    public StatusNotFoundError(Guid statusId)
        : this(message: $"Status {statusId} not found")
    {
    }
}
