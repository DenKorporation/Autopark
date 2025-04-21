using Autopark.Dal.Core.Filters;

namespace Autopark.PublicApi.Shared.Vehicles.Dto;

public class VehicleFilterDto
{
    public string? PurchaseDateFrom { get; set; }
    public string? PurchaseDateTo { get; set; }
    public RangeFilter<decimal> Cost { get; set; } = new();
    public Guid? FuelTypeId { get; set; }
}