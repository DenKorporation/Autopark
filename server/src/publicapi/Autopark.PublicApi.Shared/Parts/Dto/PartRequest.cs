using Autopark.Common.Domain;

namespace Autopark.PublicApi.Shared.Parts.Dto;

public class PartRequest : EntityDto
{
    public string Name { get; set; }
    public string Category { get; set; }
    public string Manufacturer { get; set; }
    public uint ServiceLife { get; set; }
}