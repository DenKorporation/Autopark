namespace Autopark.Common.Bl.Errors;

public class BadRequestError(string code, string message)
    : BaseError(code, message)
{
}
