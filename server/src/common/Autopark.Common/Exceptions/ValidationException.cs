using System.Net;
using Autopark.Common.Validations;

namespace Autopark.Common.Exceptions;

public class ValidationException : ClientException
{
    public ValidationException(ValidationErrorCollection errors, string message = "Данные не прошли проверку")
        : base(HttpStatusCode.BadRequest, message)
    {
        Errors = errors;
    }

    public ValidationErrorCollection Errors { get; }

    /// <inheritdoc />
    public override string ToString()
    {
        return base.ToString() + ", Ошибки: " + Errors;
    }
}
