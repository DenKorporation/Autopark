using AutoMapper;
using Autopark.Common.Extensions;
using Autopark.PublicApi.Models.Permissions;
using Autopark.PublicApi.Shared.Permissions.Dto;

namespace Autopark.PublicApi.Bl.Permissions.Mappings.Profiles;

public class PermissionProfile : Profile
{
    public PermissionProfile()
    {
        CreateMap<PermissionRequest, Permission>()
            .IgnorePropertiesNotContainedInType(typeof(PermissionRequest))
            .ForMember(dest => dest.ExpiryDate, opt => opt.MapFrom(src => DateOnly.Parse(src.ExpiryDate)));

        CreateMap<Permission, PermissionResponse>()
            .ForMember(dest => dest.IsValid, opt => opt.MapFrom(src => src.ExpiryDate >= DateOnly.FromDateTime(DateTime.Today)));
    }
}
