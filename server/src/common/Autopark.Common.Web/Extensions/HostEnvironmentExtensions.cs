using Microsoft.Extensions.Hosting;
using Environments = Autopark.Common.Constants.Environments;

namespace Autopark.Common.Web.Extensions;

public static class HostEnvironmentExtensions
{
    public static bool IsDocker(this IHostEnvironment hostEnvironment)
    {
        ArgumentNullException.ThrowIfNull(hostEnvironment);

        return hostEnvironment.IsEnvironment(Environments.Docker);
    }

    public static bool IsTesting(this IHostEnvironment hostEnvironment)
    {
        ArgumentNullException.ThrowIfNull(hostEnvironment);

        return hostEnvironment.IsEnvironment(Environments.Testing);
    }
}
