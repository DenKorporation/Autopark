using Autopark.Common.Bl.Errors;

namespace Autopark.PublicApi.Bl.PartReplacements.Errors;

public class PartReplacementNotFoundError(string code = "PartReplacement.NotFound", string message = "PartReplacement not found")
    : NotFoundError(code, message)
{
    public PartReplacementNotFoundError(Guid partReplacementId)
        : this(message: $"PartReplacement {partReplacementId} not found")
    {
    }
}
