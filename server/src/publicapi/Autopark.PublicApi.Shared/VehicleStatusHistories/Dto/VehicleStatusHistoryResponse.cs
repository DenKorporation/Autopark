namespace Autopark.PublicApi.Shared.VehicleStatusHistories.Dto;

public class VehicleStatusHistoryResponse
{
    public Guid Id { get; set; }
    public DateOnly Date { get; set; }
    public Guid VehicleId { get; set; }
    public Guid StatusId { get; set; }
}