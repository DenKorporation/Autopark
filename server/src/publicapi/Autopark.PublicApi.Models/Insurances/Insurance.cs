using Autopark.Common.Domain;
using Autopark.PublicApi.Models.Vehicles;

namespace Autopark.PublicApi.Models.Insurances;

public class Insurance : EntityBase
{
    public string Series { get; set; } = null!;
    public string Number { get; set; } = null!;
    public string VehicleType { get; set; } = null!;
    public string Provider { get; set; } = null!;
    public DateOnly IssueDate { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public decimal Cost { get; set; }
    public Guid VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;
}
