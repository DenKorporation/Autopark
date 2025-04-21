using Autopark.Common.Domain;
using Autopark.PublicApi.Models.Vehicles;

namespace Autopark.PublicApi.Models.OdometerHistories;

public class OdometerHistory : EntityBase
{
    public DateOnly Date { get; set; }
    public uint Amount { get; set; }
    public Guid VehicleId { get; set; }
    public Vehicle Vehicle { get; set; }
}
