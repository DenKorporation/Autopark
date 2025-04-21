using Autopark.Common.Bl.Errors;

namespace Autopark.PublicApi.Bl.MaintenanceRecords.Errors;

public class MaintenanceRecordNotFoundError(
    string code = "MaintenanceRecord.NotFound",
    string message = "MaintenanceRecord not found")
    : NotFoundError(code, message)
{
    public MaintenanceRecordNotFoundError(Guid maintenanceRecordId)
        : this(message: $"MaintenanceRecord '{maintenanceRecordId}' not found")
    {
    }
}
