using AutoMapper;
using Autopark.Common.Extensions;
using Autopark.PublicApi.Models.Parts;
using Autopark.PublicApi.Shared.Parts.Dto;

namespace Autopark.PublicApi.Bl.Parts.Mappings.Profiles;

public class PartProfile : Profile
{
    public PartProfile()
    {
        CreateMap<PartRequest, Part>()
            .IgnorePropertiesNotContainedInType(typeof(PartRequest));

        CreateMap<Part, PartResponse>();
    }
}
