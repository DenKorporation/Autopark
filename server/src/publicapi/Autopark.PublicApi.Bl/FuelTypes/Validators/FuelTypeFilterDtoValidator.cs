using Autopark.PublicApi.Shared.FuelTypes.Dto;
using FluentValidation;

namespace Autopark.PublicApi.Bl.FuelTypes.Validators;

public class FuelTypeFilterDtoValidator : AbstractValidator<FuelTypeFilterDto>
{
    public FuelTypeFilterDtoValidator()
    {
        RuleFor(i => i.Name.Value)
            .MaximumLength(50)
            .WithMessage("Length of name mustn't exceed 50")
            .When(x => x.Name.HasValue());
    }
}
