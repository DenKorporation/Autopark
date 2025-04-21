using Autopark.Dal.Core.Filters;
using Autopark.Dal.Core.Repositories;
using Autopark.PublicApi.Models.Vehicles;
using Autopark.PublicApi.Shared.MaintenanceRecords.Dto;
using Autopark.PublicApi.Shared.Vehicles.Dto;
using FluentValidation;

namespace Autopark.PublicApi.Bl.MaintenanceRecords.Validators;

public class MaintenanceRecordsRequestValidator : AbstractValidator<MaintenanceRecordRequest>
{
    private readonly IDtoRepository<Vehicle, VehicleResponse> _vehicleDtoRepository;

    public MaintenanceRecordsRequestValidator(IDtoRepository<Vehicle, VehicleResponse> vehicleDtoRepository)
    {
        _vehicleDtoRepository = vehicleDtoRepository;

        RuleFor(i => i.Type)
            .NotEmpty()
            .WithMessage("Maintenance type was expected")
            .MaximumLength(100)
            .WithMessage("Length of maintenance type mustn't exceed 100");

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("Maintenance start date was expected")
            .Matches(@"^\d{4}-\d{2}-\d{2}$")
            .WithMessage("Invalid date format. Expected format is 'yyyy-MM-dd'.");

        RuleFor(x => x.EndDate)
            .Matches(@"^\d{4}-\d{2}-\d{2}$")
            .WithMessage("Invalid date format. Expected format is 'yyyy-MM-dd'.")
            .When(x => x.EndDate != null);

        RuleFor(x => x.Odometer)
            .NotEmpty()
            .WithMessage("Odometer state was expected")
            .GreaterThanOrEqualTo(0U)
            .WithMessage("Odometer state shouldn't be negative");

        RuleFor(i => i.ServiceCenter)
            .NotEmpty()
            .WithMessage("Service center was expected")
            .MaximumLength(255)
            .WithMessage("Length of service center mustn't exceed 255");

        RuleFor(x => x.Cost)
            .NotEmpty()
            .WithMessage("Maintenance cost was expected")
            .GreaterThanOrEqualTo(0)
            .WithMessage("Maintenance cost shouldn't be negative");

        RuleFor(x => x.VehicleId)
            .NotEmpty()
            .WithMessage("Vehicle ID was expected")
            .MustAsync(VehicleMustExistAsync)
            .WithMessage(i => $"Vehicle '{i.VehicleId}' not found");
    }

    private async Task<bool> VehicleMustExistAsync(Guid id, CancellationToken cancellationToken)
    {
        var vehicleExist = await _vehicleDtoRepository.AnyByFilterAsync(
            new IdFilterDto<Guid>()
            {
                Ids = id
            }, cancellationToken);

        return vehicleExist;
    }
}
