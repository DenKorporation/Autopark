using Autopark.Dal.Core.FilterConverters;
using Autopark.PublicApi.Models.Statuses;
using Autopark.PublicApi.Shared.Statuses.Dto;
using Microsoft.Extensions.Logging;

namespace Autopark.PublicApi.Dal.Statuses.FilterConverters;

public class StatusFilterConverter : FilterConverterBase<Status, StatusFilterDto>
{
    public StatusFilterConverter(ILogger<FilterConverterBase<Status, StatusFilterDto>> logger) : base(logger)
    {
        HandleField(x => x.Name, x => x.Name);
    }
}
