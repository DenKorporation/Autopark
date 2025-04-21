using AutoMapper;
using Autopark.Common.Extensions;
using Autopark.PublicApi.Models.PartReplacements;
using Autopark.PublicApi.Shared.PartReplacements.Dto;

namespace Autopark.PublicApi.Bl.PartReplacements.Mappings.Profiles;

public class PartReplacementProfile : Profile
{
    public PartReplacementProfile()
    {
        CreateMap<PartReplacementRequest, PartReplacement>()
            .IgnorePropertiesNotContainedInType(typeof(PartReplacementRequest))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateOnly.Parse(src.Date)));

        CreateMap<PartReplacement, PartReplacementResponse>();
    }
}
