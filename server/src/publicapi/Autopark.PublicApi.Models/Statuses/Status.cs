using Autopark.Common.Domain;
using Autopark.PublicApi.Models.VehicleStatusHistories;

namespace Autopark.PublicApi.Models.Statuses;

public class Status : EntityBase
{
    public string Name { get; set; }
    public ICollection<VehicleStatusHistory> VehicleStatusHistories { get; set; }
}
