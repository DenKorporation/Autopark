using Autopark.Dal.Core.FilterConverters;
using Autopark.PublicApi.Models.VehicleStatusHistories;
using Autopark.PublicApi.Shared.VehicleStatusHistories.Dto;
using LinqKit;
using Microsoft.Extensions.Logging;

namespace Autopark.PublicApi.Dal.VehicleStatusHistories.FilterConverters;

public class VehicleStatusHistoryFilterConverter : FilterConverterBase<VehicleStatusHistory, VehicleStatusHistoryFilterDto>
{
    public VehicleStatusHistoryFilterConverter(ILogger<FilterConverterBase<VehicleStatusHistory, VehicleStatusHistoryFilterDto>> logger) : base(logger)
    {
        HandleField(x => x.VehicleId, x => x.VehicleId);
        HandleField(x => x.StatusId, x => x.StatusId);
        
        HandleField(x => x.DateFrom,
            (prop, value, filter) =>
            {
                if (filter.DateFrom is null)
                {
                    return DoNothing;
                }

                var date = DateOnly.Parse(filter.DateFrom);

                return PredicateBuilder.New<VehicleStatusHistory>(x => x.Date >= date);
            });
        
        HandleField(x => x.DateTo,
            (prop, value, filter) =>
            {
                if (filter.DateTo is null)
                {
                    return DoNothing;
                }

                var date = DateOnly.Parse(filter.DateTo);

                return PredicateBuilder.New<VehicleStatusHistory>(x => x.Date <= date);
            });
    }
}
