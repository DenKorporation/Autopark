using AutoMapper;
using Autopark.Common.Extensions;
using Autopark.PublicApi.Models.MaintenanceRecords;
using Autopark.PublicApi.Shared.Insurances.Dto;
using Autopark.PublicApi.Shared.MaintenanceRecords.Dto;

namespace Autopark.PublicApi.Bl.MaintenanceRecords.Mappings.Profiles;

public class MaintenanceRecordProfile : Profile
{
    public MaintenanceRecordProfile()
    {
        CreateMap<MaintenanceRecordRequest, MaintenanceRecord>()
            .IgnorePropertiesNotContainedInType(typeof(InsuranceRequest))
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => DateOnly.Parse(src.StartDate)))
            .ForMember(
                dest => dest.EndDate,
                opt => opt.MapFrom(src => src.EndDate != null ? DateOnly.Parse(src.EndDate) : (DateOnly?)null));

        CreateMap<MaintenanceRecord, MaintenanceRecordResponse>();
    }
}
