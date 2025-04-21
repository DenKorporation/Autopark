using Autopark.Common.Bl.Errors;

namespace Autopark.PublicApi.Bl.Permissions.Errors;

public class PermissionNotFoundError(string code = "Permission.NotFound", string message = "Permission not found")
    : NotFoundError(code, message)
{
    public PermissionNotFoundError(Guid permissionId)
        : this(message: $"Permission '{permissionId}' not found")
    {
    }
}
