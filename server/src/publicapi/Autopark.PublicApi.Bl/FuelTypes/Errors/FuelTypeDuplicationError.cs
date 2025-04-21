using Autopark.Common.Bl.Errors;

namespace Autopark.PublicApi.Bl.FuelTypes.Errors;

public class FuelTypeDuplicationError(
    string code = "FuelType.Duplication",
    string message = "FuelType already exist")
    : ConflictError(code, message)
{
    public FuelTypeDuplicationError(string name)
        : this(message: $"FuelType '{name}' already exist")
    {
    }
}
