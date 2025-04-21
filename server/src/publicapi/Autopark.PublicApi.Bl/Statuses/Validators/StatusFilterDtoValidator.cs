using Autopark.PublicApi.Shared.Statuses.Dto;
using FluentValidation;

namespace Autopark.PublicApi.Bl.Statuses.Validators;

public class StatusFilterDtoValidator : AbstractValidator<StatusFilterDto>
{
    public StatusFilterDtoValidator()
    {
        RuleFor(i => i.Name.Value)
            .MaximumLength(50)
            .WithMessage("Length of name mustn't exceed 50")
            .When(x => x.Name.HasValue());
    }
}
