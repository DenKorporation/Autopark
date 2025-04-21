using System.Net;
using Autopark.Common.Validations;

namespace Autopark.Common.Bl.Dtos;

public class ErrorDetails
{
    public string Message { get; set; }

    public HttpStatusCode HttpStatusCode { get; set; }

    public string Exception { get; set; }

    public ValidationErrorCollection Errors { get; set; }

    public string Tag { get; set; }
}