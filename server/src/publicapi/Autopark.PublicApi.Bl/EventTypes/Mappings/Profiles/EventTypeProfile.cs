using AutoMapper;
using Autopark.Common.Extensions;
using Autopark.PublicApi.Models.EventTypes;
using Autopark.PublicApi.Shared.EventTypes.Dto;

namespace Autopark.PublicApi.Bl.EventTypes.Mappings.Profiles;

public class EventTypeProfile : Profile
{
    public EventTypeProfile()
    {
        CreateMap<EventTypeRequest, EventType>()
            .IgnorePropertiesNotContainedInType(typeof(EventTypeRequest));

        CreateMap<EventType, EventTypeResponse>();
    }
}
