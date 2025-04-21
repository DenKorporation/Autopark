using Autopark.Dal.Core.Filters;

namespace Autopark.PublicApi.Shared.Insurances.Dto;

public class InsuranceFilterDto
{
    public StringFilter Series { get; set; } = new();
    public StringFilter Number { get; set; } = new();
    public StringFilter VehicleType { get; set; } = new();
    public StringFilter Provider { get; set; } = new();
    public string? StartDate { get; set; }
    public string? EndDate { get; set; }
    public RangeFilter<decimal> Cost { get; set; } = new();
    public Guid? VehicleId { get; set; }
    public bool? IsValid { get; set; }
}
