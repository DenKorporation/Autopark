using Autopark.Common.Bl.Errors;

namespace Autopark.PublicApi.Bl.TechnicalPassports.Errors;

public class TechnicalPassportDuplicationError(
    string code = "TechnicalPassport.Duplication",
    string message = "TechnicalPassport already exist")
    : ConflictError(code, message)
{
    public TechnicalPassportDuplicationError(Guid vehicleId)
        : this(message: $"TechnicalPassport for vehicle '{vehicleId}' already exist")
    {
    }

    public TechnicalPassportDuplicationError(string description)
        : this(message: $"TechnicalPassport '{description}' already exist")
    {
    }
}
