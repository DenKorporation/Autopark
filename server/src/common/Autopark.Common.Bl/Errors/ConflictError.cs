namespace Autopark.Common.Bl.Errors;

public class ConflictError(string code, string message)
    : BaseError(code, message)
{
}
