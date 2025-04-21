using Autopark.PublicApi.Shared.Statuses.Dto;
using FluentValidation;

namespace Autopark.PublicApi.Bl.Statuses.Validators;

public class StatusRequestValidator : AbstractValidator<StatusRequest>
{
    public StatusRequestValidator()
    {
        RuleFor(i => i.Name)
            .NotEmpty()
            .WithMessage("Status name was expected")
            .MaximumLength(50)
            .WithMessage("Length of status name mustn't exceed 50");
    }
}
