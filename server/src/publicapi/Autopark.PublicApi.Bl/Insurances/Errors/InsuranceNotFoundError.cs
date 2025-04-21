using Autopark.Common.Bl.Errors;

namespace Autopark.PublicApi.Bl.Insurances.Errors;

public class InsuranceNotFoundError(string code = "Insurance.NotFound", string message = "Insurance not found")
    : NotFoundError(code, message)
{
    public InsuranceNotFoundError(Guid insuranceId)
        : this(message: $"Insurance {insuranceId} not found")
    {
    }
}
