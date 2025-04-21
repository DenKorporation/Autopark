using Autopark.Dal.Core.FilterConverters;
using Autopark.PublicApi.Models.Permissions;
using Autopark.PublicApi.Shared.Permissions.Dto;
using LinqKit;
using Microsoft.Extensions.Logging;

namespace Autopark.PublicApi.Dal.Permissions.FilterConverters;

public class PermissionFilterConverter : FilterConverterBase<Permission, PermissionFilterDto>
{
    public PermissionFilterConverter(ILogger<FilterConverterBase<Permission, PermissionFilterDto>> logger) : base(logger)
    {
        HandleField(x => x.ExpiryDateFrom,
            (prop, value, filter) =>
            {
                if (filter.ExpiryDateFrom is null)
                {
                    return DoNothing;
                }

                var date = DateOnly.Parse(filter.ExpiryDateFrom);

                return PredicateBuilder.New<Permission>(x => x.ExpiryDate >= date);
            });
        
        HandleField(x => x.ExpiryDateTo,
            (prop, value, filter) =>
            {
                if (filter.ExpiryDateTo is null)
                {
                    return DoNothing;
                }

                var date = DateOnly.Parse(filter.ExpiryDateTo);

                return PredicateBuilder.New<Permission>(x => x.ExpiryDate <= date);
            });
        
        HandleField(x => x.IsValid,
            (prop, value, filter) =>
            {
                if (filter.IsValid is null)
                {
                    return DoNothing;
                }

                return PredicateBuilder.New<Permission>(x => x.ExpiryDate >= DateOnly.FromDateTime(DateTime.Today) == filter.IsValid);
            });
    }
}
