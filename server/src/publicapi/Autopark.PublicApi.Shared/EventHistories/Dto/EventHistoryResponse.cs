namespace Autopark.PublicApi.Shared.EventHistories.Dto;

public class EventHistoryResponse
{
    public Guid Id { get; set; }
    public DateOnly Date { get; set; }
    public string Description { get; set; }
    public Guid VehicleId { get; set; }
    public Guid EventTypeId { get; set; }
}