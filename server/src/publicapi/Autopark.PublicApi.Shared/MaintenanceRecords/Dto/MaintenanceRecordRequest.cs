using Autopark.Common.Domain;

namespace Autopark.PublicApi.Shared.MaintenanceRecords.Dto;

public class MaintenanceRecordRequest : EntityDto
{
    public string Type { get; set; }
    public string StartDate { get; set; }
    public string? EndDate { get; set; }
    public uint Odometer { get; set; }
    public string ServiceCenter { get; set; }
    public decimal Cost { get; set; }
    public string Description { get; set; }
    public Guid VehicleId { get; set; }
}
