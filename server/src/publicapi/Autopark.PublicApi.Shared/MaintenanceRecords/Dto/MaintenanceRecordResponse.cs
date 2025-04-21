namespace Autopark.PublicApi.Shared.MaintenanceRecords.Dto;

public class MaintenanceRecordResponse
{
    public Guid Id { get; set; }
    public string Type { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public uint Odometer { get; set; }
    public string ServiceCenter { get; set; }
    public decimal Cost { get; set; }
    public string Description { get; set; }
    public Guid VehicleId { get; set; }
}