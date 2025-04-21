using System.Net;

namespace Autopark.Common.Exceptions;

public class ClientException : Exception
{
    public ClientException(
        HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest,
        string message = null,
        Exception innerException = null)
        : base(message ?? "Произошла неизвестная ошибка, попробуйте повторить операцию позднее", innerException)
    {
        HttpStatusCode = httpStatusCode;
    }

    public HttpStatusCode HttpStatusCode { get; }
}
