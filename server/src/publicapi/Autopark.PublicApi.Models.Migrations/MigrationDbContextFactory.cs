using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Autopark.PublicApi.Models.Migrations;

public class MigrationDbContextFactory : IDesignTimeDbContextFactory<PublicApiDbContext>
{
    public PublicApiDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<PublicApiDbContext>();
        optionsBuilder.UseNpgsql(config.GetConnectionString("Database"), o => o.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name));

        return new PublicApiDbContext(optionsBuilder.Options);
    }
}