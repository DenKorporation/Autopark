using Autopark.PublicApi.Shared.Parts.Dto;
using FluentValidation;

namespace Autopark.PublicApi.Bl.Parts.Validators;

public class PartFilterDtoValidator : AbstractValidator<PartFilterDto>
{
    public PartFilterDtoValidator()
    {
        RuleFor(i => i.Name.Value)
            .MaximumLength(255)
            .WithMessage("Length of name mustn't exceed 255")
            .When(x => x.Name.HasValue());
        
        RuleFor(i => i.Category.Value)
            .MaximumLength(255)
            .WithMessage("Length of category mustn't exceed 255")
            .When(x => x.Category.HasValue());
        
        RuleFor(i => i.Manufacturer.Value)
            .MaximumLength(255)
            .WithMessage("Length of manufacturer mustn't exceed 255")
            .When(x => x.Manufacturer.HasValue());
    }
}
