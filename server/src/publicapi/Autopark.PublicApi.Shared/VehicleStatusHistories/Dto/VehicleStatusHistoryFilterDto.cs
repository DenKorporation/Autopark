namespace Autopark.PublicApi.Shared.VehicleStatusHistories.Dto;

public class VehicleStatusHistoryFilterDto
{
    public string? DateFrom { get; set; }
    public string? DateTo { get; set; }
    public Guid? VehicleId { get; set; }
    public Guid? StatusId { get; set; }
}