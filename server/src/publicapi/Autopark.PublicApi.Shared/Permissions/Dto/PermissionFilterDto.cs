using Autopark.Dal.Core.Filters;

namespace Autopark.PublicApi.Shared.Permissions.Dto;

public class PermissionFilterDto
{
    public StringFilter Number { get; set; } = new();
    public string? ExpiryDateFrom { get; set; }
    public string? ExpiryDateTo { get; set; }
    public Guid? VehicleId { get; set; }
    public bool? IsValid { get; set; }
}
