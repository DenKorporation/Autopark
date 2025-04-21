using Autopark.Dal.Core.Filters;

namespace Autopark.PublicApi.Shared.OdometerHistories.Dto;

public class OdometerHistoryFilterDto
{
    public string? DateFrom { get; set; }
    public string? DateTo { get; set; }
    public RangeFilter<uint> Amount { get; set; } = new();
    public Guid? VehicleId { get; set; }
}