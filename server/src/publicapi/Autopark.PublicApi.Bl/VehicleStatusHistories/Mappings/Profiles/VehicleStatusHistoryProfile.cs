using AutoMapper;
using Autopark.Common.Extensions;
using Autopark.PublicApi.Models.VehicleStatusHistories;
using Autopark.PublicApi.Shared.VehicleStatusHistories.Dto;

namespace Autopark.PublicApi.Bl.VehicleStatusHistories.Mappings.Profiles;

public class VehicleStatusHistoryProfile : Profile
{
    public VehicleStatusHistoryProfile()
    {
        CreateMap<VehicleStatusHistoryRequest, VehicleStatusHistory>()
            .IgnorePropertiesNotContainedInType(typeof(VehicleStatusHistoryRequest))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateOnly.Parse(src.Date)));

        CreateMap<VehicleStatusHistory, VehicleStatusHistoryResponse>();
    }
}
