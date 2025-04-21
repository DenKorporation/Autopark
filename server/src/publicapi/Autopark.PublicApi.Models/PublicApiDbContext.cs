using System.Reflection;
using Autopark.PublicApi.Models.EventHistories;
using Autopark.PublicApi.Models.EventTypes;
using Autopark.PublicApi.Models.FuelTypes;
using Autopark.PublicApi.Models.Insurances;
using Autopark.PublicApi.Models.MaintenanceRecords;
using Autopark.PublicApi.Models.OdometerHistories;
using Autopark.PublicApi.Models.PartReplacements;
using Autopark.PublicApi.Models.Parts;
using Autopark.PublicApi.Models.Permissions;
using Autopark.PublicApi.Models.RefuelingHistories;
using Autopark.PublicApi.Models.Statuses;
using Autopark.PublicApi.Models.TechnicalPassports;
using Autopark.PublicApi.Models.Vehicles;
using Autopark.PublicApi.Models.VehicleStatusHistories;
using Microsoft.EntityFrameworkCore;

namespace Autopark.PublicApi.Models;

public class PublicApiDbContext(DbContextOptions<PublicApiDbContext> options)
    : DbContext(options)
{
    public DbSet<FuelType> FuelTypes { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<TechnicalPassport> TechnicalPassports { get; set; }
    public DbSet<Insurance> Insurances { get; set; }
    public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<Part> Parts { get; set; }
    public DbSet<PartReplacement> PartReplacements { get; set; }
    public DbSet<OdometerHistory> OdometerHistories { get; set; }
    public DbSet<Status> Statuses { get; set; }
    public DbSet<VehicleStatusHistory> VehicleStatusHistories { get; set; }
    public DbSet<RefuelingHistory> RefuelingHistories { get; set; }
    public DbSet<EventType> EventTypes { get; set; }
    public DbSet<EventHistory> EventHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
