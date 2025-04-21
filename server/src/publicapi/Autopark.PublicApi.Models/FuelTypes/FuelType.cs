using Autopark.Common.Domain;
using Autopark.PublicApi.Models.Vehicles;

namespace Autopark.PublicApi.Models.FuelTypes;

public class FuelType : EntityBase
{
    public string Name { get; set; }
    public ICollection<Vehicle> Vehicles { get; set; } = null!;
}
