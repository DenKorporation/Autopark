namespace Autopark.PublicApi.Shared.OdometerHistories.Dto;

public class OdometerHistoryResponse
{
    public Guid Id { get; set; }
    public DateOnly Date { get; set; }
    public uint Amount { get; set; }
    public Guid VehicleId { get; set; }
}