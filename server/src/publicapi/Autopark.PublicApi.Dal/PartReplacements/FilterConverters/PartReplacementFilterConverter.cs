using Autopark.Dal.Core.FilterConverters;
using Autopark.PublicApi.Models.PartReplacements;
using Autopark.PublicApi.Shared.PartReplacements.Dto;
using LinqKit;
using Microsoft.Extensions.Logging;

namespace Autopark.PublicApi.Dal.PartReplacements.FilterConverters;

public class PartReplacementFilterConverter : FilterConverterBase<PartReplacement, PartReplacementFilterDto>
{
    public PartReplacementFilterConverter(ILogger<FilterConverterBase<PartReplacement, PartReplacementFilterDto>> logger) : base(logger)
    {
        HandleField(x => x.DateFrom,
            (prop, value, filter) =>
            {
                if (filter.DateFrom is null)
                {
                    return DoNothing;
                }

                var date = DateOnly.Parse(filter.DateFrom);

                return PredicateBuilder.New<PartReplacement>(x => x.Date >= date);
            });
        
        HandleField(x => x.DateTo,
            (prop, value, filter) =>
            {
                if (filter.DateTo is null)
                {
                    return DoNothing;
                }

                var date = DateOnly.Parse(filter.DateTo);

                return PredicateBuilder.New<PartReplacement>(x => x.Date <= date);
            });

        HandleField(x => x.Odometer, x => x.Odometer);
    }
}
