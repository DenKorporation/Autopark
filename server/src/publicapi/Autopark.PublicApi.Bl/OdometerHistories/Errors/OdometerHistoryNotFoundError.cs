using Autopark.Common.Bl.Errors;

namespace Autopark.PublicApi.Bl.OdometerHistories.Errors;

public class OdometerHistoryNotFoundError(string code = "OdometerHistory.NotFound", string message = "OdometerHistory not found")
    : NotFoundError(code, message)
{
    public OdometerHistoryNotFoundError(Guid odometerHistoryId)
        : this(message: $"OdometerHistory {odometerHistoryId} not found")
    {
    }
}
