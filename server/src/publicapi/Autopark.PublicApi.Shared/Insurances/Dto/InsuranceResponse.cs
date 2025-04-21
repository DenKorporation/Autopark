using Autopark.Common.Domain;

namespace Autopark.PublicApi.Shared.Insurances.Dto;

public class InsuranceResponse : EntityDto
{
    public string Series { get; set; }
    public string Number { get; set; }
    public string VehicleType { get; set; }
    public string Provider { get; set; }
    public DateOnly IssueDate { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public decimal Cost { get; set; }
    public Guid VehicleId { get; set; }
    public bool IsValid { get; set; }
}