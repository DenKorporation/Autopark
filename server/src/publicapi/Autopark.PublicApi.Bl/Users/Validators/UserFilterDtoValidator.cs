using Autopark.PublicApi.Shared.Users.Dto;
using FluentValidation;

namespace Autopark.PublicApi.Bl.Users.Validators;

public class UserFilterDtoValidator : AbstractValidator<UserFilterDto>
{
    public UserFilterDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email was expected")
            .EmailAddress()
            .WithMessage("Incorrect email format")
            .MaximumLength(256)
            .WithMessage("Length of email mustn't exceed 256")
            .When(x => x.Email is not null);

        RuleFor(x => x.Role)
            .NotEmpty()
            .WithMessage("Role was expected")
            .MaximumLength(256)
            .WithMessage("Length of role mustn't exceed 256")
            .When(x => x.Role is not null);

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("Firstname was expected")
            .MaximumLength(100)
            .WithMessage("Length of firstname mustn't exceed 100")
            .When(x => x.FirstName is not null);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Lastname was expected")
            .MaximumLength(100)
            .WithMessage("Length of lastname mustn't exceed 100")
            .When(x => x.LastName is not null);
    }
}
