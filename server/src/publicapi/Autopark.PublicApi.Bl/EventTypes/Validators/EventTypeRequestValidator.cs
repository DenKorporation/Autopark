using Autopark.PublicApi.Shared.EventTypes.Dto;
using FluentValidation;

namespace Autopark.PublicApi.Bl.EventTypes.Validators;

public class EventTypeRequestValidator : AbstractValidator<EventTypeRequest>
{
    public EventTypeRequestValidator()
    {
        RuleFor(i => i.Name)
            .NotEmpty()
            .WithMessage("Event type name was expected")
            .MaximumLength(50)
            .WithMessage("Length of event type name mustn't exceed 50");
    }
}
