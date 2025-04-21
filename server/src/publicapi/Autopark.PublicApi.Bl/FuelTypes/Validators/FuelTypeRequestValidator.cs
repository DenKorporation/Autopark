using Autopark.PublicApi.Shared.FuelTypes.Dto;
using FluentValidation;

namespace Autopark.PublicApi.Bl.FuelTypes.Validators;

public class FuelTypeRequestValidator : AbstractValidator<FuelTypeRequest>
{
    public FuelTypeRequestValidator()
    {
        RuleFor(i => i.Name)
            .NotEmpty()
            .WithMessage("Fuel type name was expected")
            .MaximumLength(50)
            .WithMessage("Length of fuel type name mustn't exceed 50");
    }
}
