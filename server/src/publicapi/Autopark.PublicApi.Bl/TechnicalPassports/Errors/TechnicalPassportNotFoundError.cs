using Autopark.Common.Bl.Errors;

namespace Autopark.PublicApi.Bl.TechnicalPassports.Errors;

public class TechnicalPassportNotFoundError(
    string code = "TechnicalPassport.NotFound",
    string message = "TechnicalPassport not found")
    : NotFoundError(code, message)
{
    public TechnicalPassportNotFoundError(Guid technicalPassportId)
        : this(message: $"TechnicalPassport '{technicalPassportId}' not found")
    {
    }
}
