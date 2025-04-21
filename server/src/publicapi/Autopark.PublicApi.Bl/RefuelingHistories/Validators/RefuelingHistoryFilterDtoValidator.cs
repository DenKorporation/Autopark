using Autopark.PublicApi.Shared.RefuelingHistories.Dto;
using FluentValidation;

namespace Autopark.PublicApi.Bl.RefuelingHistories.Validators;

public class RefuelingHistoryFilterDtoValidator : AbstractValidator<RefuelingHistoryFilterDto>
{
    public RefuelingHistoryFilterDtoValidator()
    {
        RuleFor(x => x.Amount.Start)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Refueling amount shouldn't be negative")
            .When(x => x.Amount.Start.HasValue);

        RuleFor(x => x.Amount.End)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Refueling amount shouldn't be negative")
            .When(x => x.Amount.End.HasValue);

        RuleFor(x => x.TotalCost.Start)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Refueling total cost amount shouldn't be negative")
            .When(x => x.TotalCost.Start.HasValue);

        RuleFor(x => x.TotalCost.End)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Refueling total shouldn't be negative")
            .When(x => x.TotalCost.End.HasValue);
        
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
