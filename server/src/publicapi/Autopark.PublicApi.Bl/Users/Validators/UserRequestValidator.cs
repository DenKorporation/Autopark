using Autopark.Common.Extensions;
using Autopark.PublicApi.Shared.Users.Dto;
using FluentValidation;

namespace Autopark.PublicApi.Bl.Users.Validators;

public class UserRequestValidator : AbstractValidator<UserRequest>
{
    public UserRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email was expected")
            .EmailAddress()
            .WithMessage("Incorrect email format")
            .MaximumLength(256)
            .WithMessage("Length of email mustn't exceed 256");

        RuleFor(x => x.Role)
            .NotEmpty()
            .WithMessage("Role was expected")
            .MaximumLength(256)
            .WithMessage("Length of role mustn't exceed 256")
            .Must(RoleMustExist)
            .WithMessage(x => $"Role {x.Role} did not exist");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("Firstname was expected")
            .MaximumLength(100)
            .WithMessage("Length of firstname mustn't exceed 100");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Lastname was expected")
            .MaximumLength(100)
            .WithMessage("Length of lastname mustn't exceed 100");
    }

    private bool RoleMustExist(string role)
    {
        var allRoles = RoleExtensions.GetAllRoles();

        return allRoles.Contains(role);
    }
}
