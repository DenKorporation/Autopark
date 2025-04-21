using Autopark.Common.Bl.Errors;

namespace Autopark.PublicApi.Bl.Insurances.Errors;

public class InsuranceDuplicationError(
    string code = "Insurance.Duplication",
    string message = "Insurance already exist")
    : ConflictError(code, message)
{
    public InsuranceDuplicationError(string seriesAndNumber)
        : this(message: $"Insurance '{seriesAndNumber}' already exist")
    {
    }
}
