using Autopark.Dal.Core.Filters;

namespace Autopark.PublicApi.Shared.EventHistories.Dto;

public class EventHistoryFilterDto
{
    public string? DateFrom { get; set; }
    public string? DateTo { get; set; }
    public StringFilter Description { get; set; } = new();
    public Guid? VehicleId { get; set; }
    public Guid? EventTypeId { get; set; }
}