using Autopark.Dal.Core.Filters;

namespace Autopark.PublicApi.Shared.Parts.Dto;

public class PartFilterDto
{
    public StringFilter Name { get; set; } = new();
    public StringFilter Category { get; set; } = new();
    public StringFilter Manufacturer { get; set; } = new();
    public RangeFilter<uint> ServiceLife { get; set; } = new();
}