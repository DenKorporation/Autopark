using Autopark.Dal.Core.FilterConverters;
using Autopark.PublicApi.Models.EventTypes;
using Autopark.PublicApi.Shared.EventTypes.Dto;
using Microsoft.Extensions.Logging;

namespace Autopark.PublicApi.Dal.EventTypes.FilterConverters;

public class EventTypeFilterConverter : FilterConverterBase<EventType, EventTypeFilterDto>
{
    public EventTypeFilterConverter(ILogger<FilterConverterBase<EventType, EventTypeFilterDto>> logger) : base(logger)
    {
        HandleField(x => x.Name, x => x.Name);
    }
}
