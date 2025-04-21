using Autopark.PublicApi.Shared.Vehicles.Dto;
using FluentValidation;

namespace Autopark.PublicApi.Bl.Vehicles.Validators;

public class VehicleFilterDtoValidator : AbstractValidator<VehicleFilterDto>
{
    public VehicleFilterDtoValidator()
    {
        RuleFor(x => x.PurchaseDateFrom)
            .Matches(@"^\d{4}-\d{2}-\d{2}$")
            .WithMessage("Invalid date format. Expected format is 'yyyy-MM-dd'.")
            .When(x => x.PurchaseDateFrom is not null);

        RuleFor(x => x.PurchaseDateTo)
            .Matches(@"^\d{4}-\d{2}-\d{2}$")
            .WithMessage("Invalid date format. Expected format is 'yyyy-MM-dd'.")
            .When(x => x.PurchaseDateTo is not null);

        RuleFor(x => x.Cost.Start)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Vehicle cost shouldn't be negative")
            .When(x => x.Cost.Start.HasValue);

        RuleFor(x => x.Cost.End)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Vehicle cost shouldn't be negative")
            .When(x => x.Cost.End.HasValue);
    }
}
