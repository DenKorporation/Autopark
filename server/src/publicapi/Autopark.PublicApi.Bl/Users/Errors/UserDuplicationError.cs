using Autopark.Common.Bl.Errors;

namespace Autopark.PublicApi.Bl.Users.Errors;

public class UserDuplicationError(
    string code = "User.Duplication",
    string message = "User already exist")
    : ConflictError(code, message)
{
    public UserDuplicationError(string email)
        : this(message: $"User '{email}' already exist")
    {
    }
}
