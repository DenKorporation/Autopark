using Autopark.PublicApi.Shared.OdometerHistories.Dto;
using FluentValidation;

namespace Autopark.PublicApi.Bl.OdometerHistories.Validators;

public class OdometerHistoryFilterDtoValidator : AbstractValidator<OdometerHistoryFilterDto>
{
    public OdometerHistoryFilterDtoValidator()
    {
        RuleFor(x => x.DateFrom)
            .Matches(@"^\d{4}-\d{2}-\d{2}$")
            .WithMessage("Invalid date format. Expected format is 'yyyy-MM-dd'.")
            .When(x => x.DateFrom is not null);

        RuleFor(x => x.DateTo)
            .Matches(@"^\d{4}-\d{2}-\d{2}$")
            .WithMessage("Invalid date format. Expected format is 'yyyy-MM-dd'.")
            .When(x => x.DateTo is not null);
    }
}
