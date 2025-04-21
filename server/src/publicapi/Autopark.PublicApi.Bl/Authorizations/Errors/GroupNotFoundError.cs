using Autopark.Common.Bl.Errors;

namespace Autopark.PublicApi.Bl.Authorizations.Errors;

public class GroupNotFoundError(string code = "Group.NotFound", string message = "Group not found")
    : NotFoundError(code, message)
{
    public GroupNotFoundError(Guid groupId)
        : this(message: $"Group {groupId} not found")
    {
    }
}
