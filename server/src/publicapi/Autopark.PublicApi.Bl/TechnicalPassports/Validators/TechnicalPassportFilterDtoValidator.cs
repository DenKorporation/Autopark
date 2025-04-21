using Autopark.PublicApi.Shared.TechnicalPassports.Dto;
using FluentValidation;

namespace Autopark.PublicApi.Bl.TechnicalPassports.Validators;

public class TechnicalPassportFilterDtoValidator : AbstractValidator<TechnicalPassportFilterDto>
{
    public TechnicalPassportFilterDtoValidator()
    {
        RuleFor(x => x.Number.Value)
            .MaximumLength(9)
            .WithMessage("Length of technical passport number mustn't exceed 9")
            .When(x => x.Number.HasValue());

        RuleFor(x => x.FirstName.Value)
            .MaximumLength(50)
            .WithMessage("Length of first name mustn't exceed 50")
            .When(x => x.FirstName.HasValue());

        RuleFor(x => x.LastName.Value)
            .MaximumLength(50)
            .WithMessage("Length of last name mustn't exceed 50")
            .When(x => x.LastName.HasValue());

        RuleFor(x => x.IssueDateFrom)
            .Matches(@"^\d{4}-\d{2}-\d{2}$")
            .WithMessage("Invalid date format. Expected format is 'yyyy-MM-dd'.")
            .When(x => x.IssueDateFrom is not null);

        RuleFor(x => x.IssueDateTo)
            .Matches(@"^\d{4}-\d{2}-\d{2}$")
            .WithMessage("Invalid date format. Expected format is 'yyyy-MM-dd'.")
            .When(x => x.IssueDateTo is not null);

        RuleFor(x => x.SAICode.Value)
            .MaximumLength(6)
            .WithMessage("Length of technical passport SAI code mustn't exceed 6")
            .When(x => x.SAICode.HasValue());

        RuleFor(x => x.LicensePlate.Value)
            .MaximumLength(8)
            .WithMessage("Length of license plate mustn't exceed 8")
            .When(x => x.LicensePlate.HasValue());

        RuleFor(x => x.Brand.Value)
            .MaximumLength(30)
            .WithMessage("Length of vehicle brand mustn't exceed 30")
            .When(x => x.Brand.HasValue());

        RuleFor(x => x.Model.Value)
            .MaximumLength(50)
            .WithMessage("Length of vehicle model mustn't exceed 50")
            .When(x => x.Model.HasValue());

        RuleFor(x => x.CreationYear.Start)
            .GreaterThan(1900U)
            .WithMessage("Creation year of vehicle greater than 1900")
            .When(x => x.CreationYear.Start.HasValue);

        RuleFor(x => x.CreationYear.End)
            .GreaterThan(1900U)
            .WithMessage("Creation year of vehicle greater than 1900")
            .When(x => x.CreationYear.End.HasValue);

        RuleFor(x => x.VIN.Value)
            .MaximumLength(17)
            .WithMessage("Length of technical passport VIN code mustn't exceed 17")
            .When(x => x.VIN.HasValue());

        RuleFor(x => x.MaxWeight.Start)
            .GreaterThanOrEqualTo(0U)
            .WithMessage("Vehicle max weight shouldn't be negative")
            .When(x => x.MaxWeight.Start.HasValue);

        RuleFor(x => x.MaxWeight.End)
            .GreaterThanOrEqualTo(0U)
            .WithMessage("Vehicle max weight shouldn't be negative")
            .When(x => x.MaxWeight.End.HasValue);
    }
}
