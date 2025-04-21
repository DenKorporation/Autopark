using Autopark.Common.Domain;

namespace Autopark.PublicApi.Shared.RefuelingHistories.Dto;

public class RefuelingHistoryRequest : EntityDto
{
    public decimal Amount { get; set; }
    public decimal TotalCost { get; set; }
    public string Date { get; set; }
    public Guid VehicleId { get; set; }
}