namespace Autopark.Common.Bl.Errors;

public class InternalServerError(string code, string message = "Something went wrong")
    : BaseError(code, message)
{
}
