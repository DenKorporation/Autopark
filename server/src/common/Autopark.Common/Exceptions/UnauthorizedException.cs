using System.Net;

namespace Autopark.Common.Exceptions;

public class UnauthorizedException : ClientException
{
    public UnauthorizedException(string message = "Необходимо авторизоваться")
        : base(HttpStatusCode.Unauthorized, message)
    {
    }
}
