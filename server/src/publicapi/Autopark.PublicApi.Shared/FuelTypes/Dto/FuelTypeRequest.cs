using Autopark.Common.Domain;

namespace Autopark.PublicApi.Shared.FuelTypes.Dto;

public class FuelTypeRequest : EntityDto
{
    public string Name { get; set; }
}