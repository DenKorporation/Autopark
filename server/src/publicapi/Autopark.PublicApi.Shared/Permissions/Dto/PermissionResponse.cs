using Autopark.Common.Domain;

namespace Autopark.PublicApi.Shared.Permissions.Dto;

public class PermissionResponse : EntityDto
{
    public string Number { get; set; }
    public DateOnly ExpiryDate { get; set; }
    public Guid VehicleId { get; set; }
    public bool IsValid { get; set; }
}
