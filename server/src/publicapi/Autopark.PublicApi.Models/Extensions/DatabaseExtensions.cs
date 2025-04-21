using Autopark.PublicApi.Models.Initializers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Autopark.PublicApi.Models.Extensions;

public static class DatabaseExtensions
{
    public static async Task<IHost> InitializeDatabaseAsync(
        this IHost app,
        IList<Guid> groupIds,
        CancellationToken cancellationToken = default)
    {
        using var scope = app.Services.CreateScope();

        var initializer = scope.ServiceProvider.GetRequiredService<AutoParkDbContextInitializer>();

        await initializer.InitializeAsync(cancellationToken);

        await initializer.SeedAsync(groupIds, cancellationToken);

        return app;
    }
}
