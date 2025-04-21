using AutoMapper;
using Autopark.Common.Extensions;
using Autopark.PublicApi.Models.Statuses;
using Autopark.PublicApi.Shared.Statuses.Dto;

namespace Autopark.PublicApi.Bl.Statuses.Mappings.Profiles;

public class StatusProfile : Profile
{
    public StatusProfile()
    {
        CreateMap<StatusRequest, Status>()
            .IgnorePropertiesNotContainedInType(typeof(StatusRequest));

        CreateMap<Status, StatusResponse>();
    }
}
