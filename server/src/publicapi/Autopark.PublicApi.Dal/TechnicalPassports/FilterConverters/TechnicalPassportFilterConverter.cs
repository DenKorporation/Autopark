using Autopark.Dal.Core.FilterConverters;
using Autopark.PublicApi.Models.TechnicalPassports;
using Autopark.PublicApi.Shared.TechnicalPassports.Dto;
using LinqKit;
using Microsoft.Extensions.Logging;

namespace Autopark.PublicApi.Dal.TechnicalPassports.FilterConverters;

public class TechnicalPassportFilterConverter : FilterConverterBase<TechnicalPassport, TechnicalPassportFilterDto>
{
    public TechnicalPassportFilterConverter(ILogger<FilterConverterBase<TechnicalPassport, TechnicalPassportFilterDto>> logger) : base(logger)
    {
        HandleField(x => x.IssueDateFrom,
            (prop, value, filter) =>
            {
                if (filter.IssueDateFrom is null)
                {
                    return DoNothing;
                }

                var date = DateOnly.Parse(filter.IssueDateFrom);

                return PredicateBuilder.New<TechnicalPassport>(x => x.IssueDate >= date);
            });
        
        HandleField(x => x.IssueDateTo,
            (prop, value, filter) =>
            {
                if (filter.IssueDateTo is null)
                {
                    return DoNothing;
                }

                var date = DateOnly.Parse(filter.IssueDateTo);

                return PredicateBuilder.New<TechnicalPassport>(x => x.IssueDate <= date);
            });
        
       HandleField(x => x.CreationYear, x => x.CreationYear);
       HandleField(x => x.MaxWeight, x => x.MaxWeight);
    }
}
