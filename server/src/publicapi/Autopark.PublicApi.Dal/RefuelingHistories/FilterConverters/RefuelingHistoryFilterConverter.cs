using Autopark.Dal.Core.FilterConverters;
using Autopark.PublicApi.Models.RefuelingHistories;
using Autopark.PublicApi.Shared.RefuelingHistories.Dto;
using LinqKit;
using Microsoft.Extensions.Logging;

namespace Autopark.PublicApi.Dal.RefuelingHistories.FilterConverters;

public class RefuelingHistoryFilterConverter : FilterConverterBase<RefuelingHistory, RefuelingHistoryFilterDto>
{
    public RefuelingHistoryFilterConverter(ILogger<FilterConverterBase<RefuelingHistory, RefuelingHistoryFilterDto>> logger) : base(logger)
    {
        HandleField(x => x.Amount, x => x.Amount);
        HandleField(x => x.TotalCost, x => x.TotalCost);
        HandleField(x => x.VehicleId, x => x.VehicleId);
        HandleField(x => x.DateFrom,
            (prop, value, filter) =>
            {
                if (filter.DateFrom is null)
                {
                    return DoNothing;
                }

                var date = DateOnly.Parse(filter.DateFrom);

                return PredicateBuilder.New<RefuelingHistory>(x => x.Date >= date);
            });
        
        HandleField(x => x.DateTo,
            (prop, value, filter) =>
            {
                if (filter.DateTo is null)
                {
                    return DoNothing;
                }

                var date = DateOnly.Parse(filter.DateTo);

                return PredicateBuilder.New<RefuelingHistory>(x => x.Date <= date);
            });
    }
}
