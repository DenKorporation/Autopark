using Autopark.Dal.Core.Filters;

namespace Autopark.PublicApi.Shared.RefuelingHistories.Dto;

public class RefuelingHistoryFilterDto
{
    public RangeFilter<decimal> Amount { get; set; } = new();
    public RangeFilter<decimal> TotalCost { get; set; } = new();
    public string? DateFrom { get; set; }
    public string? DateTo { get; set; }
    public Guid? VehicleId { get; set; }
}