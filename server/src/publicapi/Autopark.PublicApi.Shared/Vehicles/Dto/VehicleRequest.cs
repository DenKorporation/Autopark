using Autopark.Common.Domain;

namespace Autopark.PublicApi.Shared.Vehicles.Dto;

public class VehicleRequest : EntityDto
{
    public string PurchaseDate { get; set; }
    public decimal Cost { get; set; }
    public Guid FuelTypeId { get; set; }
}