using AutoMapper;
using Autopark.Common.Extensions;
using Autopark.PublicApi.Shared.Users.Dto;
using FS.Keycloak.RestApiClient.Model;

namespace Autopark.PublicApi.Bl.Users.Mappings.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserRepresentation, UserResponse>()
            .ForMember(x => x.Role, opt => opt.Ignore())
            .ForMember(x => x.Id, x => x.MapFrom(d => ParseGuid(d.Id)));

        CreateMap<UserRequest, UserRepresentation>()
            .IgnorePropertiesNotContainedInType(typeof(UserRequest));
    }
    
    private Guid? ParseGuid(string id)
    {
        if (Guid.TryParse(id, out var guid))
        {
            return guid == Guid.Empty ? null : guid;
        }

        return null;
    }
}
