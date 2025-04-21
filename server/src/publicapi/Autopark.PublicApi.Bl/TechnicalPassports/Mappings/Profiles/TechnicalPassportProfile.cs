using AutoMapper;
using Autopark.Common.Extensions;
using Autopark.PublicApi.Models.TechnicalPassports;
using Autopark.PublicApi.Shared.TechnicalPassports.Dto;

namespace Autopark.PublicApi.Bl.TechnicalPassports.Mappings.Profiles;

public class TechnicalPassportProfile : Profile
{
    public TechnicalPassportProfile()
    {
        CreateMap<TechnicalPassportRequest, TechnicalPassport>()
            .IgnorePropertiesNotContainedInType(typeof(TechnicalPassportRequest))
            .ForMember(dest => dest.IssueDate, opt => opt.MapFrom(src => DateOnly.Parse(src.IssueDate)))
            .ForMember(dest => dest.Vehicle, opt => opt.Ignore());

        CreateMap<TechnicalPassport, TechnicalPassportResponse>();
    }
}
