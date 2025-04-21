using FluentResults;

namespace Autopark.Common.Bl.Errors;

public abstract class BaseError(string code, string message)
    : Error(message)
{
    public string Code { get; set; } = code;
}
