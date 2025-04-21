using Autopark.Dal.Core.Filters;

namespace Autopark.PublicApi.Shared.TechnicalPassports.Dto;

public class TechnicalPassportFilterDto
{
    public StringFilter Number { get; set; } = new();
    public StringFilter FirstName { get; set; } = new();
    public StringFilter LastName { get; set; } = new();
    public string? IssueDateFrom { get; set; }
    public string? IssueDateTo { get; set; }
    public StringFilter SAICode { get; set; } = new();
    public StringFilter LicensePlate { get; set; } = new();
    public StringFilter Brand { get; set; } = new();
    public StringFilter Model { get; set; } = new();
    public RangeFilter<uint> CreationYear { get; set; } = new();
    public StringFilter VIN { get; set; }  = new();
    public RangeFilter<uint> MaxWeight { get; set; } = new();
    public Guid? VehicleId { get; set; }    
}
