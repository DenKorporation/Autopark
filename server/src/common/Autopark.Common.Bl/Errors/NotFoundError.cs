namespace Autopark.Common.Bl.Errors;

public class NotFoundError(string code, string message)
    : BaseError(code, message)
{
}
