using Autopark.Dal.Core.FilterConverters;
using Autopark.PublicApi.Models.Vehicles;
using Autopark.PublicApi.Shared.Vehicles.Dto;
using LinqKit;
using Microsoft.Extensions.Logging;

namespace Autopark.PublicApi.Dal.Vehicles.FilterConverters;

public class VehicleFilterConverter : FilterConverterBase<Vehicle, VehicleFilterDto>
{
    public VehicleFilterConverter(ILogger<FilterConverterBase<Vehicle, VehicleFilterDto>> logger) : base(logger)
    {
        HandleField(x => x.PurchaseDateFrom,
            (prop, value, filter) =>
            {
                if (filter.PurchaseDateFrom is null)
                {
                    return DoNothing;
                }

                var date = DateOnly.Parse(filter.PurchaseDateFrom);

                return PredicateBuilder.New<Vehicle>(x => x.PurchaseDate >= date);
            });
        
        HandleField(x => x.PurchaseDateTo,
            (prop, value, filter) =>
            {
                if (filter.PurchaseDateTo is null)
                {
                    return DoNothing;
                }

                var date = DateOnly.Parse(filter.PurchaseDateTo);

                return PredicateBuilder.New<Vehicle>(x => x.PurchaseDate <= date);
            });
        
        HandleField(x => x.Cost, x => x.Cost);
        HandleField(x => x.FuelTypeId, x => x.FuelTypeId);
    }
}
