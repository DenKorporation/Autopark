using Autopark.Common.Bl.Errors;

namespace Autopark.PublicApi.Bl.Users.Errors;

public class UserNotFoundError(string code = "User.NotFound", string message = "User not found")
    : NotFoundError(code, message)
{
    public UserNotFoundError(Guid userId)
        : this(message: $"User '{userId}' not found")
    {
    }
    
    public UserNotFoundError(string email)
        : this(message: $"User '{email}' not found")
    {
    }
}
