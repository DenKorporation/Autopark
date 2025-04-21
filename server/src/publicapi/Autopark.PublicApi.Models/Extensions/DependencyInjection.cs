using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Autopark.PublicApi.Models.Extensions;

public static class DependencyInjection
{
    public static void AddPublicApiDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PublicApiDbContext>(
            options =>
                options.UseNpgsql(configuration.GetConnectionString("Database")));
    }
}
