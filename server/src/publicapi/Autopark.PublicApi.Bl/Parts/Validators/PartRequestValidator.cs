using Autopark.PublicApi.Shared.Parts.Dto;
using FluentValidation;

namespace Autopark.PublicApi.Bl.Parts.Validators;

public class PartRequestValidator : AbstractValidator<PartRequest>
{
    public PartRequestValidator()
    {
        RuleFor(i => i.Name)
            .NotEmpty()
            .WithMessage("Part name was expected")
            .MaximumLength(255)
            .WithMessage("Length of part name mustn't exceed 255");

        RuleFor(i => i.Category)
            .NotEmpty()
            .WithMessage("Part category was expected")
            .MaximumLength(255)
            .WithMessage("Length of part category mustn't exceed 255");

        RuleFor(i => i.Manufacturer)
            .NotEmpty()
            .WithMessage("Part manufacturer was expected")
            .MaximumLength(255)
            .WithMessage("Length of part manufacturer mustn't exceed 255");

        RuleFor(x => x.ServiceLife)
            .NotEmpty()
            .WithMessage("Part service life was expected");
    }
}
