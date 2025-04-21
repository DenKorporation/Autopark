using Autopark.Common.Bl.Errors;

namespace Autopark.PublicApi.Bl.VehicleStatusHistories.Errors;

public class VehicleStatusHistoryNotFoundError(string code = "VehicleStatusHistory.NotFound", string message = "VehicleStatusHistory not found")
    : NotFoundError(code, message)
{
    public VehicleStatusHistoryNotFoundError(Guid vehicleStatusHistoryId)
        : this(message: $"VehicleStatusHistory {vehicleStatusHistoryId} not found")
    {
    }
}
