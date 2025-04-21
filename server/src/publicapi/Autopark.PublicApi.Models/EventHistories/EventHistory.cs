using Autopark.Common.Domain;
using Autopark.PublicApi.Models.EventTypes;
using Autopark.PublicApi.Models.Vehicles;

namespace Autopark.PublicApi.Models.EventHistories;

public class EventHistory : EntityBase
{
    public DateOnly Date { get; set; }
    public string Description { get; set; }
    public Guid VehicleId { get; set; }
    public Guid EventTypeId { get; set; }
    public Vehicle Vehicle { get; set; }
    public EventType EventType { get; set; }
}
