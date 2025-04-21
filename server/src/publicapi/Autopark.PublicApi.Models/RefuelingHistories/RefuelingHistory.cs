using Autopark.Common.Domain;
using Autopark.PublicApi.Models.Vehicles;

namespace Autopark.PublicApi.Models.RefuelingHistories;

public class RefuelingHistory : EntityBase
{
    public decimal Amount { get; set; }
    public decimal TotalCost { get; set; }
    public DateOnly Date { get; set; }
    public Guid VehicleId { get; set; }
    public Vehicle Vehicle { get; set; }
}
