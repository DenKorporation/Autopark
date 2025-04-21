namespace Autopark.PublicApi.Shared.Vehicles.Dto;

public class VehicleResponse
{
    public Guid Id { get; set; }
    public DateOnly PurchaseDate { get; set; }
    public decimal Cost { get; set; }
    public Guid FuelTypeId { get; set; }

}