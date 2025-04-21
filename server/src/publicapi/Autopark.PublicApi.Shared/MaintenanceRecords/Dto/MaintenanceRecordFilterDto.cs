using Autopark.Dal.Core.Filters;

namespace Autopark.PublicApi.Shared.MaintenanceRecords.Dto;

public class MaintenanceRecordFilterDto
{
    public StringFilter Type { get; set; } = new();
    public string? StartDate { get; set; }
    public string? EndDate { get; set; }
    public RangeFilter<int> Odometer { get; set; } = new();
    public StringFilter ServiceCenter { get; set; } = new();
    public StringFilter Description { get; set; } = new();
    public RangeFilter<decimal> Cost { get; set; } = new();
    public Guid? VehicleId { get; set; }
}
