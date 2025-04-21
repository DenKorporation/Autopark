using System.Net;

namespace Autopark.Common.Exceptions;

public class NotFoundException : ClientException
{
    public NotFoundException(string message = null, Exception innerException = null)
        : base(HttpStatusCode.NotFound, message, innerException)
    {
    }
}