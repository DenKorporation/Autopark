using Autopark.Common.Domain;
using Autopark.PublicApi.Models.Vehicles;

namespace Autopark.PublicApi.Models.MaintenanceRecords;

public class MaintenanceRecord : EntityBase
{
    public string Type { get; set; } = null!;
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public uint Odometer { get; set; }
    public string ServiceCenter { get; set; } = null!;
    public decimal Cost { get; set; }
    public string Description { get; set; }
    public Guid VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;
}
