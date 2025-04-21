namespace Autopark.PublicApi.Shared.PartReplacements.Dto;

public class PartReplacementResponse
{
    public Guid Id { get; set; }
    public Guid VehicleId { get; set; }
    public Guid PartId { get; set; }
    public DateOnly Date { get; set; }
    public uint Odometer { get; set; }
}