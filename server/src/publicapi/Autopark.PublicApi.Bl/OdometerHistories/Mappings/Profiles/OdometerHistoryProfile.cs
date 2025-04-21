using AutoMapper;
using Autopark.Common.Extensions;
using Autopark.PublicApi.Models.OdometerHistories;
using Autopark.PublicApi.Shared.OdometerHistories.Dto;

namespace Autopark.PublicApi.Bl.OdometerHistories.Mappings.Profiles;

public class OdometerHistoryProfile : Profile
{
    public OdometerHistoryProfile()
    {
        CreateMap<OdometerHistoryRequest, OdometerHistory>()
            .IgnorePropertiesNotContainedInType(typeof(OdometerHistoryRequest))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateOnly.Parse(src.Date)));

        CreateMap<OdometerHistory, OdometerHistoryResponse>();
    }
}
