using Autopark.PublicApi.Shared.Insurances.Dto;
using FluentValidation;

namespace Autopark.PublicApi.Bl.Insurances.Validators;

public class InsuranceFilterDtoValidator : AbstractValidator<InsuranceFilterDto>
{
    public InsuranceFilterDtoValidator()
    {
        RuleFor(i => i.Series.Value)
            .MaximumLength(2)
            .WithMessage("Length of insurance series mustn't exceed 2")
            .When(x => x.Series.HasValue());

        RuleFor(i => i.Number.Value)
            .MaximumLength(7)
            .WithMessage("Length of insurance number mustn't exceed 7")
            .When(x => x.Number.HasValue());

        RuleFor(i => i.VehicleType.Value)
            .MaximumLength(2)
            .WithMessage("Length of vehicle type mustn't exceed 2")
            .When(x => x.VehicleType.HasValue());

        RuleFor(i => i.Provider.Value)
            .MaximumLength(255)
            .WithMessage("Length of insurance provider mustn't exceed 255")
            .When(x => x.Provider.HasValue());

        RuleFor(x => x.StartDate)
            .Matches(@"^\d{4}-\d{2}-\d{2}$")
            .WithMessage("Invalid date format. Expected format is 'yyyy-MM-dd'.")
            .When(x => x.StartDate is not null);

        RuleFor(x => x.EndDate)
            .Matches(@"^\d{4}-\d{2}-\d{2}$")
            .WithMessage("Invalid date format. Expected format is 'yyyy-MM-dd'.")
            .When(x => x.EndDate is not null);

        RuleFor(x => x.Cost.Start)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Insurance cost shouldn't be negative")
            .When(x => x.Cost.Start.HasValue);

        RuleFor(x => x.Cost.End)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Insurance cost shouldn't be negative")
            .When(x => x.Cost.End.HasValue);
    }
}
