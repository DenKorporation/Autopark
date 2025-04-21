using Autopark.Dal.Core.FilterConverters;
using Autopark.PublicApi.Models.Parts;
using Autopark.PublicApi.Shared.Parts.Dto;
using Microsoft.Extensions.Logging;

namespace Autopark.PublicApi.Dal.Parts.FilterConverters;

public class PartFilterConverter : FilterConverterBase<Part, PartFilterDto>
{
    public PartFilterConverter(ILogger<FilterConverterBase<Part, PartFilterDto>> logger) : base(logger)
    {
        HandleField(x => x.Name, x => x.Name);
        HandleField(x => x.Category, x => x.Category);
        HandleField(x => x.Manufacturer, x => x.Manufacturer);
        HandleField(x => x.ServiceLife, x => x.ServiceLife);
    }
}
