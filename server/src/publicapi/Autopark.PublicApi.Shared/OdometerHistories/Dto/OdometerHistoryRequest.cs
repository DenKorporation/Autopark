using Autopark.Common.Domain;

namespace Autopark.PublicApi.Shared.OdometerHistories.Dto;

public class OdometerHistoryRequest : EntityDto
{
    public string Date { get; set; }
    public uint Amount { get; set; }
    public Guid VehicleId { get; set; }
}