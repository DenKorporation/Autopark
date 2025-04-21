using Autopark.Common.Domain;

namespace Autopark.PublicApi.Shared.EventHistories.Dto;

public class EventHistoryRequest : EntityDto
{
    public string Date { get; set; }
    public string Description { get; set; }
    public Guid VehicleId { get; set; }
    public Guid EventTypeId { get; set; }
}