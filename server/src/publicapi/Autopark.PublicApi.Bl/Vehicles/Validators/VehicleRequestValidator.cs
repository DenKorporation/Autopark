using Autopark.Dal.Core.Filters;
using Autopark.Dal.Core.Repositories;
using Autopark.PublicApi.Models.FuelTypes;
using Autopark.PublicApi.Shared.FuelTypes.Dto;
using Autopark.PublicApi.Shared.Vehicles.Dto;
using FluentValidation;

namespace Autopark.PublicApi.Bl.Vehicles.Validators;

public class VehicleRequestValidator : AbstractValidator<VehicleRequest>
{
    private readonly IDtoRepository<FuelType, FuelTypeResponse> _fuelTypeDtoRepository;
    public VehicleRequestValidator(IDtoRepository<FuelType, FuelTypeResponse> fuelTypeDtoRepository)
    {
        _fuelTypeDtoRepository = fuelTypeDtoRepository;

        RuleFor(x => x.PurchaseDate)
            .NotEmpty()
            .WithMessage("Vehicle purchase date was expected")
            .Matches(@"^\d{4}-\d{2}-\d{2}$")
            .WithMessage("Invalid date format. Expected format is 'yyyy-MM-dd'.");

        RuleFor(x => x.Cost)
            .NotEmpty()
            .WithMessage("Vehicle cost was expected")
            .GreaterThanOrEqualTo(0)
            .WithMessage("Vehicle cost shouldn't be negative");
        
        RuleFor(x => x.FuelTypeId)
            .NotEmpty()
            .WithMessage("Fuel type ID was expected")
            .MustAsync(FuelTypeMustExistAsync)
            .WithMessage(i => $"Fuel type '{i.FuelTypeId}' not found");
    }

    private async Task<bool> FuelTypeMustExistAsync(Guid id, CancellationToken cancellationToken)
    {
        var fuelTypeExist = await _fuelTypeDtoRepository.AnyByFilterAsync(
            new IdFilterDto<Guid>()
            {
                Ids = id
            }, cancellationToken);

        return fuelTypeExist;
    }
}
