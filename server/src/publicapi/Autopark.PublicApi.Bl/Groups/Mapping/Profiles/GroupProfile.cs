using AutoMapper;
using Autopark.PublicApi.Shared.Groups.Dto;
using FS.Keycloak.RestApiClient.Model;

namespace Autopark.PublicApi.Bl.Groups.Mapping.Profiles;

public class GroupProfile : Profile
{
    public GroupProfile()
    {
        CreateMap<GroupRepresentation, GroupResponse>()
            .ForMember(x => x.Id, x => x.MapFrom(d => ParseGuid(d.Id)))
            .ForMember(x => x.ParentId, x => x.MapFrom(d => ParseGuid(d.ParentId)));
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
