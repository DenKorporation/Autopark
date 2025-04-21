using Autopark.Common.Domain;
using Autopark.PublicApi.Models.EventHistories;

namespace Autopark.PublicApi.Models.EventTypes;

public class EventType : EntityBase
{
    public string Name { get; set; }
    public ICollection<EventHistory> EventHistories { get; set; }
}
