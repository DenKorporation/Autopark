using Autopark.PublicApi.Shared.MaintenanceRecords.Dto;
using FluentValidation;

namespace Autopark.PublicApi.Bl.MaintenanceRecords.Validators;

public class MaintenanceRecordFilterDtoValidator : AbstractValidator<MaintenanceRecordFilterDto>
{
    public MaintenanceRecordFilterDtoValidator()
    {
        RuleFor(mr => mr.Type.Value)
            .MaximumLength(100)
            .WithMessage("Length of maintenance type mustn't exceed 100")
            .When(x => x.Type.HasValue());

        RuleFor(x => x.StartDate)
            .Matches(@"^\d{4}-\d{2}-\d{2}$")
            .WithMessage("Invalid date format. Expected format is 'yyyy-MM-dd'.")
            .When(x => x.StartDate is not null);

        RuleFor(x => x.EndDate)
            .Matches(@"^\d{4}-\d{2}-\d{2}$")
            .WithMessage("Invalid date format. Expected format is 'yyyy-MM-dd'.")
            .When(x => x.EndDate is not null);

        RuleFor(x => x.Odometer.Start)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Vehicle odometer status shouldn't be negative")
            .When(x => x.Odometer.Start.HasValue);

        RuleFor(x => x.Odometer.End)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Vehicle odometer status shouldn't be negative")
            .When(x => x.Odometer.End.HasValue);

        RuleFor(mr => mr.ServiceCenter.Value)
            .MaximumLength(255)
            .WithMessage("Length of service center mustn't exceed 255")
            .When(x => x.ServiceCenter.HasValue());

        RuleFor(x => x.Cost.Start)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Maintenance cost shouldn't be negative")
            .When(x => x.Cost.Start.HasValue);

        RuleFor(x => x.Cost.End)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Maintenance cost shouldn't be negative")
            .When(x => x.Cost.End.HasValue);
    }
}
