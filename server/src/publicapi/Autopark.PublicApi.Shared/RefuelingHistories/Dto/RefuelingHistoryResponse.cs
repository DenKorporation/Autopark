namespace Autopark.PublicApi.Shared.RefuelingHistories.Dto;

public class RefuelingHistoryResponse
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public decimal TotalCost { get; set; }
    public DateOnly Date { get; set; }
    public Guid VehicleId { get; set; }
}