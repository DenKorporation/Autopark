using Autopark.Dal.Core.FilterConverters;
using Autopark.PublicApi.Models.MaintenanceRecords;
using Autopark.PublicApi.Shared.MaintenanceRecords.Dto;
using LinqKit;
using Microsoft.Extensions.Logging;

namespace Autopark.PublicApi.Dal.MaintenanceRecords.FilterConverters;

public class MaintenanceRecordFilterConverter : FilterConverterBase<MaintenanceRecord, MaintenanceRecordFilterDto>
{
    public MaintenanceRecordFilterConverter(ILogger<FilterConverterBase<MaintenanceRecord, MaintenanceRecordFilterDto>> logger) : base(logger)
    {
        HandleField(x => x.StartDate,
            (prop, value, filter) =>
            {
                if (filter.StartDate is null)
                {
                    return DoNothing;
                }

                var date = DateOnly.Parse(filter.StartDate);

                return PredicateBuilder.New<MaintenanceRecord>(x => x.StartDate >= date);
            });
        
        HandleField(x => x.EndDate,
            (prop, value, filter) =>
            {
                if (filter.EndDate is null)
                {
                    return DoNothing;
                }

                var date = DateOnly.Parse(filter.EndDate);

                return PredicateBuilder.New<MaintenanceRecord>(x => x.EndDate <= date);
            });
        
        HandleField(x => x.Cost, x => x.Cost);
        HandleField(x => x.Odometer, x => x.Odometer);
    }
}
