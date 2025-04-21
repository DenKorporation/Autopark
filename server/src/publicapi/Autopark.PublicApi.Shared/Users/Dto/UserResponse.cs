using Autopark.Common.Domain;

namespace Autopark.PublicApi.Shared.Users.Dto;

public class UserResponse : EntityDto
{
    public string Email { get; set; } = null!;

    public string? Role { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;
}
