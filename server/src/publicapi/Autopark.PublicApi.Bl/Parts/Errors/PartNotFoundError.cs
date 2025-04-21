using Autopark.Common.Bl.Errors;

namespace Autopark.PublicApi.Bl.Parts.Errors;

public class PartNotFoundError(string code = "Part.NotFound", string message = "Part not found")
    : NotFoundError(code, message)
{
    public PartNotFoundError(Guid partId)
        : this(message: $"Part {partId} not found")
    {
    }
}
