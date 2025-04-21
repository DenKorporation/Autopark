using Autopark.Dal.Core.Filters;
using Autopark.Dal.Core.Repositories;
using Autopark.PublicApi.Models.Statuses;
using Autopark.PublicApi.Models.Vehicles;
using Autopark.PublicApi.Shared.Statuses.Dto;
using Autopark.PublicApi.Shared.Vehicles.Dto;
using Autopark.PublicApi.Shared.VehicleStatusHistories.Dto;
using FluentValidation;

namespace Autopark.PublicApi.Bl.VehicleStatusHistories.Validators;

public class VehicleStatusHistoryRequestValidator : AbstractValidator<VehicleStatusHistoryRequest>
{
    private readonly IDtoRepository<Vehicle, VehicleResponse> _vehicleDtoRepository;
    private readonly IDtoRepository<Status, StatusResponse> _statusDtoRepository;

    public VehicleStatusHistoryRequestValidator(IDtoRepository<Status, StatusResponse> statusDtoRepository, IDtoRepository<Vehicle, VehicleResponse> vehicleDtoRepository)
    {
        _statusDtoRepository = statusDtoRepository;
        _vehicleDtoRepository = vehicleDtoRepository;
        RuleFor(x => x.Date)
            .NotEmpty()
            .WithMessage("Status history date was expected")
            .Matches(@"^\d{4}-\d{2}-\d{2}$")
            .WithMessage("Invalid date format. Expected format is 'yyyy-MM-dd'.");

        RuleFor(x => x.VehicleId)
            .NotEmpty()
            .WithMessage("Vehicle ID was expected")
            .MustAsync(VehicleMustExistAsync)
            .WithMessage(i => $"Vehicle '{i.VehicleId}' not found");

        RuleFor(x => x.StatusId)
            .NotEmpty()
            .WithMessage("Status ID was expected")
            .MustAsync(StatusMustExistAsync)
            .WithMessage(i => $"Status '{i.StatusId}' not found");
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

    private async Task<bool> StatusMustExistAsync(Guid id, CancellationToken cancellationToken)
    {
        var statusExist = await _statusDtoRepository.AnyByFilterAsync(
            new IdFilterDto<Guid>()
            {
                Ids = id
            }, cancellationToken);

        return statusExist;
    }
}
