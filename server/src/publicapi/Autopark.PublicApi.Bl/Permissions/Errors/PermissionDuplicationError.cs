using Autopark.Common.Bl.Errors;

namespace Autopark.PublicApi.Bl.Permissions.Errors;

public class PermissionDuplicationError(
    string code = "Permission.Duplication",
    string message = "Permission already exist")
    : ConflictError(code, message)
{
    public PermissionDuplicationError(Guid vehicleId)
        : this(message: $"Permission for vehicle '{vehicleId}' already exist")
    {
    }

    public PermissionDuplicationError(string number)
        : this(message: $"Permission '{number}' already exist")
    {
    }
}
