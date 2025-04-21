using AutoMapper;
using Autopark.Common.Extensions;
using Autopark.PublicApi.Models.Insurances;
using Autopark.PublicApi.Shared.Insurances.Dto;

namespace Autopark.PublicApi.Bl.Insurances.Mappings.Profiles;

public class InsuranceProfile : Profile
{
    public InsuranceProfile()
    {
        CreateMap<InsuranceRequest, Insurance>()
            .IgnorePropertiesNotContainedInType(typeof(InsuranceRequest))
            .ForMember(dest => dest.IssueDate, opt => opt.MapFrom(src => DateOnly.Parse(src.IssueDate)))
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => DateOnly.Parse(src.StartDate)))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => DateOnly.Parse(src.EndDate)));

        CreateMap<Insurance, InsuranceResponse>()
            .ForMember(dest => dest.IsValid, opt => opt.MapFrom(src => src.EndDate >= DateOnly.FromDateTime(DateTime.Today)));
    }
}
