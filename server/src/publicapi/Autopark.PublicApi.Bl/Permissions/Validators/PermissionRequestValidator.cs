using Autopark.Dal.Core.Filters;
using Autopark.Dal.Core.Repositories;
using Autopark.PublicApi.Models.Vehicles;
using Autopark.PublicApi.Shared.Permissions.Dto;
using Autopark.PublicApi.Shared.Vehicles.Dto;
using FluentValidation;

namespace Autopark.PublicApi.Bl.Permissions.Validators;

public class PermissionRequestValidator : AbstractValidator<PermissionRequest>
{
    private readonly IDtoRepository<Vehicle, VehicleResponse> _vehicleDtoRepository;

    public PermissionRequestValidator(IDtoRepository<Vehicle, VehicleResponse> vehicleDtoRepository)
    {
        _vehicleDtoRepository = vehicleDtoRepository;
        RuleFor(i => i.Number)
            .NotEmpty()
            .WithMessage("Permission number was expected")
            .Length(9)
            .WithMessage("Length of permission number was expected to be 9")
            .Matches("^[a-zA-Z]{2}[0-9]{7}$")
            .WithMessage("Permission number must contain 2 alphabetic letters and 7 digits");

        RuleFor(x => x.ExpiryDate)
            .NotEmpty()
            .WithMessage("Permission expiry date was expected")
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
