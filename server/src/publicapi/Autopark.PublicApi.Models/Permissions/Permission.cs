using Autopark.Common.Domain;
using Autopark.PublicApi.Models.Vehicles;

namespace Autopark.PublicApi.Models.Permissions;

public class Permission : EntityBase
{
    public string Number { get; set; } = null!;
    public DateOnly ExpiryDate { get; set; }
    public Guid VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;
}
