using Autopark.Common.Domain;

namespace Autopark.PublicApi.Shared.VehicleStatusHistories.Dto;

public class VehicleStatusHistoryRequest : EntityDto
{
    public string Date { get; set; }
    public Guid VehicleId { get; set; }
    public Guid StatusId { get; set; }
}