using Autopark.Common.Web.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Autopark.Common.Web.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        return app.UseMiddleware<ErrorDetailsHandlerMiddleware>();
    }
}
