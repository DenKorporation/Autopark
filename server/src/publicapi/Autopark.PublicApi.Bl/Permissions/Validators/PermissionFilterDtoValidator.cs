using Autopark.PublicApi.Shared.Permissions.Dto;
using FluentValidation;

namespace Autopark.PublicApi.Bl.Permissions.Validators;

public class PermissionFilterDtoValidator : AbstractValidator<PermissionFilterDto>
{
    public PermissionFilterDtoValidator()
    {
        RuleFor(p => p.Number.Value)
            .MaximumLength(9)
            .WithMessage("Length of permission number mustn't exceed 9")
            .When(x => x.Number.HasValue());

        RuleFor(x => x.ExpiryDateFrom)
            .Matches(@"^\d{4}-\d{2}-\d{2}$")
            .WithMessage("Invalid date format. Expected format is 'yyyy-MM-dd'.")
            .When(x => x.ExpiryDateFrom is not null);

        RuleFor(x => x.ExpiryDateTo)
            .Matches(@"^\d{4}-\d{2}-\d{2}$")
            .WithMessage("Invalid date format. Expected format is 'yyyy-MM-dd'.")
            .When(x => x.ExpiryDateTo is not null);
    }
}
