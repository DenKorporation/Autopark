using System.Net;

namespace Autopark.Common.Exceptions;

public class ForbiddenException : ClientException
{
    public ForbiddenException(string message = "Недостаточно полномочий для совершения операции")
        : base(HttpStatusCode.Forbidden, message)
    {
    }
}