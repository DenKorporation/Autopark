using Autopark.Common.Domain;

namespace Autopark.PublicApi.Shared.PartReplacements.Dto;


public class PartReplacementRequest : EntityDto
{
    public Guid VehicleId { get; set; }
    public Guid PartId { get; set; }
    public string Date { get; set; }
    public uint Odometer { get; set; }
}