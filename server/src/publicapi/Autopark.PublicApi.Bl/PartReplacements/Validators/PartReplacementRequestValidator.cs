using Autopark.Dal.Core.Filters;
using Autopark.Dal.Core.Repositories;
using Autopark.PublicApi.Models.Parts;
using Autopark.PublicApi.Models.Vehicles;
using Autopark.PublicApi.Shared.PartReplacements.Dto;
using Autopark.PublicApi.Shared.Parts.Dto;
using Autopark.PublicApi.Shared.Vehicles.Dto;
using FluentValidation;

namespace Autopark.PublicApi.Bl.PartReplacements.Validators;

public class PartReplacementRequestValidator : AbstractValidator<PartReplacementRequest>
{
    private readonly IDtoRepository<Vehicle, VehicleResponse> _vehicleDtoRepository;
    private readonly IDtoRepository<Part, PartResponse> _partDtoRepository;

    public PartReplacementRequestValidator(IDtoRepository<Vehicle, VehicleResponse> vehicleDtoRepository, IDtoRepository<Part, PartResponse> partDtoRepository)
    {
        _vehicleDtoRepository = vehicleDtoRepository;
        _partDtoRepository = partDtoRepository;
        RuleFor(x => x.Odometer)
            .NotEmpty()
            .WithMessage("Odometer value was expected");

        RuleFor(x => x.Date)
            .NotEmpty()
            .WithMessage("Part replacement date was expected")
            .Matches(@"^\d{4}-\d{2}-\d{2}$")
            .WithMessage("Invalid date format. Expected format is 'yyyy-MM-dd'.");

        RuleFor(x => x.VehicleId)
            .NotEmpty()
            .WithMessage("Vehicle ID was expected")
            .MustAsync(VehicleMustExistAsync)
            .WithMessage(i => $"Vehicle '{i.VehicleId}' not found");

        RuleFor(x => x.PartId)
            .NotEmpty()
            .WithMessage("Part ID was expected")            
            .MustAsync(PartMustExistAsync)
            .WithMessage(i => $"Part '{i.PartId}' not found");
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

    private async Task<bool> PartMustExistAsync(Guid id, CancellationToken cancellationToken)
    {
        var partExist = await _partDtoRepository.AnyByFilterAsync(
            new IdFilterDto<Guid>()
            {
                Ids = id
            }, cancellationToken);

        return partExist;
    }
}
