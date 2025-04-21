using Autopark.Common.Bl.Errors;

namespace Autopark.PublicApi.Bl.Vehicles.Errors;

public class VehicleNotFoundError(string code = "Vehicle.NotFound", string message = "Vehicle not found")
    : NotFoundError(code, message)
{
    public VehicleNotFoundError(Guid vehicleId)
        : this(message: $"Vehicle '{vehicleId}' not found")
    {
    }
}
