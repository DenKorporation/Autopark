using Autopark.Common.Domain;
using Autopark.PublicApi.Models.Parts;
using Autopark.PublicApi.Models.Vehicles;

namespace Autopark.PublicApi.Models.PartReplacements;

public class PartReplacement : EntityBase
{
    public Guid VehicleId { get; set; }
    public Guid PartId { get; set; }
    public DateOnly Date { get; set; }
    public uint Odometer { get; set; }
    public Part Part { get; set; }
    public Vehicle Vehicle { get; set; }
}
