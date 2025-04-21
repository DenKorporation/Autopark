using Autopark.Dal.Core.FilterConverters;
using Autopark.PublicApi.Models.EventHistories;
using Autopark.PublicApi.Models.Insurances;
using Autopark.PublicApi.Shared.EventHistories.Dto;
using Autopark.PublicApi.Shared.Insurances.Dto;
using LinqKit;
using Microsoft.Extensions.Logging;

namespace Autopark.PublicApi.Dal.EventHistories.FilterConverters;

public class EventHistoryFilterConverter : FilterConverterBase<EventHistory, EventHistoryFilterDto>
{
    public EventHistoryFilterConverter(ILogger<FilterConverterBase<EventHistory, EventHistoryFilterDto>> logger) : base(logger)
    {
        HandleField(x => x.DateFrom,
            (prop, value, filter) =>
            {
                if (filter.DateFrom is null)
                {
                    return DoNothing;
                }

                var date = DateOnly.Parse(filter.DateFrom);

                return PredicateBuilder.New<EventHistory>(x => x.Date >= date);
            });
        
        HandleField(x => x.DateTo,
            (prop, value, filter) =>
            {
                if (filter.DateTo is null)
                {
                    return DoNothing;
                }

                var date = DateOnly.Parse(filter.DateTo);

                return PredicateBuilder.New<EventHistory>(x => x.Date <= date);
            });
        
        HandleField(x => x.Description, x => x.Description);
        HandleField(x => x.VehicleId, x => x.VehicleId);
        HandleField(x => x.EventTypeId, x => x.EventTypeId);
    }
}
