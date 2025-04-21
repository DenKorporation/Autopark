using Autopark.PublicApi.Bl.Initializers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Autopark.PublicApi.Bl.Extensions;

public static class DatabaseExtensions
{
    public static async Task<IList<Guid>> InitializeKeycloakUsersAsync(
        this IHost app,
        CancellationToken cancellationToken = default)
    {
        using var scope = app.Services.CreateScope();

        var initializer = scope.ServiceProvider.GetRequiredService<KeycloakInitializer>();

        return await initializer.SeedAsync(cancellationToken);
    }
}
