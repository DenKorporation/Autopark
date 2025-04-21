using AutoMapper;
using Autopark.Common.Extensions;
using Autopark.PublicApi.Models.EventHistories;
using Autopark.PublicApi.Shared.EventHistories.Dto;

namespace Autopark.PublicApi.Bl.EventHistories.Mappings.Profiles;

public class EventHistoryProfile : Profile
{
    public EventHistoryProfile()
    {
        CreateMap<EventHistoryRequest, EventHistory>()
            .IgnorePropertiesNotContainedInType(typeof(EventHistoryRequest))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateOnly.Parse(src.Date)));

        CreateMap<EventHistory, EventHistoryResponse>();
    }
}
