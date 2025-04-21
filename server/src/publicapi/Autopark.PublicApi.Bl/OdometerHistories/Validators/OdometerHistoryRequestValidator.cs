using Autopark.Dal.Core.Filters;
using Autopark.Dal.Core.Repositories;
using Autopark.PublicApi.Models.Vehicles;
using Autopark.PublicApi.Shared.OdometerHistories.Dto;
using Autopark.PublicApi.Shared.Vehicles.Dto;
using FluentValidation;

namespace Autopark.PublicApi.Bl.OdometerHistories.Validators;

public class OdometerHistoryRequestValidator : AbstractValidator<OdometerHistoryRequest>
{
    private readonly IDtoRepository<Vehicle, VehicleResponse> _vehicleDtoRepository;

    public OdometerHistoryRequestValidator(IDtoRepository<Vehicle, VehicleResponse> vehicleDtoRepository)
    {
        _vehicleDtoRepository = vehicleDtoRepository;
        RuleFor(x => x.Amount)
            .NotEmpty()
            .WithMessage("Odometer amount was expected");

        RuleFor(x => x.Date)
            .NotEmpty()
            .WithMessage("Event history date was expected")
            .Matches(@"^\d{4}-\d{2}-\d{2}$")
            .WithMessage("Invalid date format. Expected format is 'yyyy-MM-dd'.");

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
