using Autopark.Dal.Core.Filters;

namespace Autopark.PublicApi.Shared.PartReplacements.Dto;

public class PartReplacementFilterDto
{
    public Guid? VehicleId { get; set; }
    public Guid? PartId { get; set; }
    public string? DateFrom { get; set; }
    public string? DateTo { get; set; }
    public RangeFilter<uint> Odometer { get; set; } = new();
}