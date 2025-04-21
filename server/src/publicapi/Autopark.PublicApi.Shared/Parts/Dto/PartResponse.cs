namespace Autopark.PublicApi.Shared.Parts.Dto;

public class PartResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public string Manufacturer { get; set; }
    public uint ServiceLife { get; set; }
}
