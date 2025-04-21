using Autopark.Common.Domain;

namespace Autopark.PublicApi.Shared.Insurances.Dto;

public class InsuranceRequest : EntityDto
{
    public string Series { get; set; }
    public string Number { get; set; }
    public string VehicleType { get; set; }
    public string Provider { get; set; }
    public string IssueDate { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public decimal Cost { get; set; }
    public Guid VehicleId { get; set; }
}
