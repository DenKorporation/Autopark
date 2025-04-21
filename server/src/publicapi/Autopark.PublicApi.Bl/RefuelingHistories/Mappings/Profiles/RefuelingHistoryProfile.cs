using AutoMapper;
using Autopark.Common.Extensions;
using Autopark.PublicApi.Models.RefuelingHistories;
using Autopark.PublicApi.Shared.RefuelingHistories.Dto;

namespace Autopark.PublicApi.Bl.RefuelingHistories.Mappings.Profiles;

public class RefuelingHistoryProfile : Profile
{
    public RefuelingHistoryProfile()
    {
        CreateMap<RefuelingHistoryRequest, RefuelingHistory>()
            .IgnorePropertiesNotContainedInType(typeof(RefuelingHistoryRequest))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateOnly.Parse(src.Date)));

        CreateMap<RefuelingHistory, RefuelingHistoryResponse>();
    }
}
