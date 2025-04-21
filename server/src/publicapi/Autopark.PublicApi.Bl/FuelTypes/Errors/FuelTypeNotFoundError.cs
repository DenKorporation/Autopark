using Autopark.Common.Bl.Errors;

namespace Autopark.PublicApi.Bl.FuelTypes.Errors;

public class FuelTypeNotFoundError(string code = "FuelType.NotFound", string message = "FuelType not found")
    : NotFoundError(code, message)
{
    public FuelTypeNotFoundError(Guid fuelTypeId)
        : this(message: $"FuelType {fuelTypeId} not found")
    {
    }
}
