using AutoMapper;
using Autopark.Common.Extensions;
using Autopark.PublicApi.Models.Vehicles;
using Autopark.PublicApi.Shared.Vehicles.Dto;

namespace Autopark.PublicApi.Bl.Vehicles.Mappings.Profiles;

public class VehicleProfile : Profile
{
    public VehicleProfile()
    {
        CreateMap<VehicleRequest, Vehicle>()
            .IgnorePropertiesNotContainedInType(typeof(VehicleRequest))
            .ForMember(dest => dest.PurchaseDate, opt => opt.MapFrom(src => DateOnly.Parse(src.PurchaseDate)));

        CreateMap<Vehicle, VehicleResponse>();
    }
}
