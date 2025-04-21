using Autopark.Common.Domain;
using Autopark.PublicApi.Models.EventHistories;
using Autopark.PublicApi.Models.FuelTypes;
using Autopark.PublicApi.Models.Insurances;
using Autopark.PublicApi.Models.MaintenanceRecords;
using Autopark.PublicApi.Models.OdometerHistories;
using Autopark.PublicApi.Models.PartReplacements;
using Autopark.PublicApi.Models.Permissions;
using Autopark.PublicApi.Models.RefuelingHistories;
using Autopark.PublicApi.Models.TechnicalPassports;
using Autopark.PublicApi.Models.VehicleStatusHistories;

namespace Autopark.PublicApi.Models.Vehicles;

public class Vehicle : EntityBase
{
    public DateOnly PurchaseDate { get; set; }
    public decimal Cost { get; set; }
    public Guid FuelTypeId { get; set; }
    public FuelType FuelType { get; set; }
    public TechnicalPassport TechnicalPassport { get; set; }
    public ICollection<Permission> Permissions { get; set; }
    public ICollection<Insurance> Insurances { get; set; } = null!;
    public ICollection<MaintenanceRecord> MaintenanceRecords { get; set; } = null!;
    public ICollection<PartReplacement> PartReplacements { get; set; }
    public ICollection<OdometerHistory> OdometerHistories { get; set; }
    public ICollection<VehicleStatusHistory> VehicleStatusHistories { get; set; }
    public ICollection<RefuelingHistory> RefuelingHistories { get; set; }
    public ICollection<EventHistory> EventHistories { get; set; }
}
