using Autopark.PublicApi.Shared.EventHistories.Dto;
using FluentValidation;

namespace Autopark.PublicApi.Bl.EventHistories.Validators;

public class EventHistoryFilterDtoValidator : AbstractValidator<EventHistoryFilterDto>
{
    public EventHistoryFilterDtoValidator()
    {
        RuleFor(i => i.Description.Value)
            .MaximumLength(255)
            .WithMessage("Length of description mustn't exceed 255")
            .When(x => x.Description.HasValue());

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
