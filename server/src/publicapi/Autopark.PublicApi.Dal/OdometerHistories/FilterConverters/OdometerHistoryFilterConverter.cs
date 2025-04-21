using Autopark.Dal.Core.FilterConverters;
using Autopark.PublicApi.Models.OdometerHistories;
using Autopark.PublicApi.Shared.OdometerHistories.Dto;
using LinqKit;
using Microsoft.Extensions.Logging;

namespace Autopark.PublicApi.Dal.OdometerHistories.FilterConverters;

public class OdometerHistoryFilterConverter : FilterConverterBase<OdometerHistory, OdometerHistoryFilterDto>
{
    public OdometerHistoryFilterConverter(ILogger<FilterConverterBase<OdometerHistory, OdometerHistoryFilterDto>> logger)
        : base(logger)
    {
        HandleField(x => x.DateFrom,
            (prop, value, filter) =>
            {
                if (filter.DateFrom is null)
                {
                    return DoNothing;
                }

                var date = DateOnly.Parse(filter.DateFrom);

                return PredicateBuilder.New<OdometerHistory>(x => x.Date >= date);
            });
        
        HandleField(x => x.DateTo,
            (prop, value, filter) =>
            {
                if (filter.DateTo is null)
                {
                    return DoNothing;
                }

                var date = DateOnly.Parse(filter.DateTo);

                return PredicateBuilder.New<OdometerHistory>(x => x.Date <= date);
            });

        HandleField(x => x.Amount, x => x.Amount);
        HandleField(x => x.VehicleId, x => x.VehicleId);
    }
}
