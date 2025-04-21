using Autopark.Dal.Core.Filters;

namespace Autopark.PublicApi.Shared.FuelTypes.Dto;

public class FuelTypeFilterDto
{
    public StringFilter Name { get; set; } = new();
}