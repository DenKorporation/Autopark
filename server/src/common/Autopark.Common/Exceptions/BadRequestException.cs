using System.Net;

namespace Autopark.Common.Exceptions;

public class BadRequestException : ClientException
{
    public BadRequestException(string message = "Переданы неправильные данные")
        : base(HttpStatusCode.BadRequest, message)
    {
    }
}