using Autopark.PublicApi.Shared.EventTypes.Dto;
using FluentValidation;

namespace Autopark.PublicApi.Bl.EventTypes.Validators;

public class EventTypeFilterDtoValidator : AbstractValidator<EventTypeFilterDto>
{
    public EventTypeFilterDtoValidator()
    {
        RuleFor(i => i.Name.Value)
            .MaximumLength(50)
            .WithMessage("Length of name mustn't exceed 50")
            .When(x => x.Name.HasValue());
    }
}
