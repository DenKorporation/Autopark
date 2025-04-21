using Autopark.Dal.Core.FilterConverters;
using Autopark.PublicApi.Models.Insurances;
using Autopark.PublicApi.Shared.Insurances.Dto;
using LinqKit;
using Microsoft.Extensions.Logging;

namespace Autopark.PublicApi.Dal.Insurances.FilterConverters;

public class InsuranceFilterConverter : FilterConverterBase<Insurance, InsuranceFilterDto>
{
    public InsuranceFilterConverter(ILogger<FilterConverterBase<Insurance, InsuranceFilterDto>> logger)
        : base(logger)
    {
        HandleField(x => x.StartDate,
            (prop, value, filter) =>
            {
                if (filter.StartDate is null)
                {
                    return DoNothing;
                }

                var date = DateOnly.Parse(filter.StartDate);

                return PredicateBuilder.New<Insurance>(x => x.StartDate >= date);
            });
        
        HandleField(x => x.EndDate,
            (prop, value, filter) =>
            {
                if (filter.EndDate is null)
                {
                    return DoNothing;
                }

                var date = DateOnly.Parse(filter.EndDate);

                return PredicateBuilder.New<Insurance>(x => x.EndDate <= date);
            });

        HandleField(x => x.Cost, x => x.Cost);

        HandleField(x => x.IsValid,
            (prop, value, filter) =>
            {
                if (filter.IsValid is null)
                {
                    return DoNothing;
                }

                return PredicateBuilder.New<Insurance>(x => x.EndDate >= DateOnly.FromDateTime(DateTime.Today) == filter.IsValid);
            });
    }
}
