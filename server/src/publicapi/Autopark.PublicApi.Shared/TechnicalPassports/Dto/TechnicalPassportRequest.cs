using Autopark.Common.Domain;

namespace Autopark.PublicApi.Shared.TechnicalPassports.Dto;

public class TechnicalPassportRequest : EntityDto
{
    public string Number { get; set; }
    public string FirstName { get; set; }
    public string FirstNameLatin { get; set; }
    public string LastName { get; set; }
    public string LastNameLatin { get; set; }
    public string Patronymic { get; set; }
    public string Address { get; set; }
    public string IssueDate { get; set; }
    public string SAICode { get; set; }
    public string LicensePlate { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public uint CreationYear { get; set; }
    public string Color { get; set; }
    public string VIN { get; set; }
    public string VehicleType { get; set; }
    public uint MaxWeight { get; set; }
    public Guid VehicleId { get; set; }
}
