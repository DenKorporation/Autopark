using Autopark.Common.Domain;
using Autopark.PublicApi.Models.Statuses;
using Autopark.PublicApi.Models.Vehicles;

namespace Autopark.PublicApi.Models.VehicleStatusHistories;

public class VehicleStatusHistory : EntityBase
{
    public DateOnly Date { get; set; }
    public Guid VehicleId { get; set; }
    public Guid StatusId { get; set; }
    public Vehicle Vehicle { get; set; }
    public Status Status { get; set; }
}
