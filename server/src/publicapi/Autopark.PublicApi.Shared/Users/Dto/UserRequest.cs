using Autopark.Common.Domain;

namespace Autopark.PublicApi.Shared.Users.Dto;

public class UserRequest : EntityDto
{
    public string Email { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Password { get; set; } = null!;
}
