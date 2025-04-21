using System.Diagnostics;
using System.Net;
using Autopark.Common.Bl.Dtos;
using Autopark.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Autopark.Common.Web.Middleware;

public class ErrorDetailsHandlerMiddleware : IMiddleware
{
    public const string TraceHeader = "Trace-Id";
    private readonly ILogger<ErrorDetailsHandlerMiddleware> logger;

    public ErrorDetailsHandlerMiddleware(
        ILogger<ErrorDetailsHandlerMiddleware> logger)
    {
        this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        ErrorDetails errorDetails = null;

        try
        {
            var traceId = Activity.Current?.TraceId;

            if (traceId != null)
            {
                context.Response.Headers[TraceHeader] = traceId.ToString();
            }

            await next.Invoke(context);

            if (context.Response.StatusCode >= 400)
            {
                logger.LogInformation(
                    $"Received {context.Response.StatusCode} http status code response without exception");

                if (!context.Response.HasStarted)
                {
                    errorDetails = new ErrorDetails
                    {
                        HttpStatusCode = (HttpStatusCode)context.Response.StatusCode
                    };

                    if (context.Response.Body.CanRead)
                    {
                        using (var bodyReader = new StreamReader(context.Response.Body))
                        {
                            var bodyAsText = await bodyReader.ReadToEndAsync();
                            errorDetails.Message = bodyAsText;
                        }
                    }
                }
            }
        }
        catch (ValidationException ex)
        {
            errorDetails = new ErrorDetails
            {
                Errors = ex.Errors,
                Message = ex.Message,
                Exception = ex.ToString(),
                HttpStatusCode = ex.HttpStatusCode,
                Tag = "validation"
            };

            logger.LogError(ex, $"Ошибка валидации. {ex.Message}");
        }
        catch (ClientException ex)
        {
            errorDetails = new ErrorDetails
            {
                Message = ex.Message,
                Exception = ex.ToString(),
                HttpStatusCode = ex.HttpStatusCode
            };

            logger.LogInformation(ex, "Client error occured");
        }
        catch (Exception ex)
        {
            errorDetails = new ErrorDetails
            {
                Exception = ex.ToString(),
                HttpStatusCode = HttpStatusCode.InternalServerError
            };

            logger.LogError(ex, ex.Message);
        }

        if (errorDetails != null)
        {
            if (string.IsNullOrWhiteSpace(errorDetails.Message))
            {
                errorDetails.Message = "Произошла неизвестная ошибка, попробуйте повторить операцию позднее";
            }

            if (context.Response.HasStarted)
            {
                logger.LogError($"Response already has started, cannot send {nameof(ErrorDetails)} object");
            }
            else
            {
                context.Response.StatusCode = (int)errorDetails.HttpStatusCode;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(errorDetails));
            }
        }
    }
}