using Autopark.Dal.Core.FilterConverters;
using Autopark.PublicApi.Models.FuelTypes;
using Autopark.PublicApi.Shared.FuelTypes.Dto;
using Microsoft.Extensions.Logging;

namespace Autopark.PublicApi.Dal.FuelTypes.FilterConverters;

public class FuelTypeFilterConverter : FilterConverterBase<FuelType, FuelTypeFilterDto>
{
    public FuelTypeFilterConverter(ILogger<FilterConverterBase<FuelType, FuelTypeFilterDto>> logger) : base(logger)
    {
        HandleField(x => x.Name, x => x.Name);
    }
}
