using AutoMapper;
using Autopark.Common.Extensions;
using Autopark.PublicApi.Models.FuelTypes;
using Autopark.PublicApi.Shared.FuelTypes.Dto;

namespace Autopark.PublicApi.Bl.FuelTypes.Mappings.Profiles;

public class FuelTypesProfile : Profile
{
    public FuelTypesProfile()
    {
        CreateMap<FuelTypeRequest, FuelType>()
            .IgnorePropertiesNotContainedInType(typeof(FuelTypeRequest));

        CreateMap<FuelType, FuelTypeResponse>();
    }
}
