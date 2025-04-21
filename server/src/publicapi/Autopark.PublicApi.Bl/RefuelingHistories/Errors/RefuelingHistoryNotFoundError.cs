using Autopark.Common.Bl.Errors;

namespace Autopark.PublicApi.Bl.RefuelingHistories.Errors;

public class RefuelingHistoryNotFoundError(string code = "RefuelingHistory.NotFound", string message = "RefuelingHistory not found")
    : NotFoundError(code, message)
{
    public RefuelingHistoryNotFoundError(Guid refuelingHistoryId)
        : this(message: $"RefuelingHistory {refuelingHistoryId} not found")
    {
    }
}
