using Autopark.Common.Domain;

namespace Autopark.PublicApi.Shared.Permissions.Dto;

public class PermissionRequest : EntityDto
{
    public string Number { get; set; }
    public string ExpiryDate { get; set; }
    public Guid VehicleId { get; set; }
}
