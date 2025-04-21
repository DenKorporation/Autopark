using Autopark.Dal.Core.Filters;
using Autopark.Dal.Core.Repositories;
using Autopark.PublicApi.Models.EventTypes;
using Autopark.PublicApi.Models.Vehicles;
using Autopark.PublicApi.Shared.EventHistories.Dto;
using Autopark.PublicApi.Shared.EventTypes.Dto;
using Autopark.PublicApi.Shared.Vehicles.Dto;
using FluentValidation;

namespace Autopark.PublicApi.Bl.EventHistories.Validators;

public class EventHistoryRequestValidator : AbstractValidator<EventHistoryRequest>
{
    private readonly IDtoRepository<Vehicle, VehicleResponse> _vehicleDtoRepository;
    private readonly IDtoRepository<EventType, EventTypeResponse> _eventTypeDtoRepository;

    public EventHistoryRequestValidator(IDtoRepository<EventType, EventTypeResponse> eventTypeDtoRepository, IDtoRepository<Vehicle, VehicleResponse> vehicleDtoRepository)
    {
        _eventTypeDtoRepository = eventTypeDtoRepository;
        _vehicleDtoRepository = vehicleDtoRepository;
        RuleFor(i => i.Description)
            .NotEmpty()
            .WithMessage("Event history description was expected")
            .MaximumLength(255)
            .WithMessage("Length of event history description mustn't exceed 255");

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
        
        RuleFor(x => x.EventTypeId)
            .NotEmpty()
            .WithMessage("EventType ID was expected")
            .MustAsync(EventTypeMustExistAsync)
            .WithMessage(i => $"EventType '{i.EventTypeId}' not found");
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

    private async Task<bool> EventTypeMustExistAsync(Guid id, CancellationToken cancellationToken)
    {
        var eventTypeExist = await _eventTypeDtoRepository.AnyByFilterAsync(
            new IdFilterDto<Guid>()
            {
                Ids = id
            }, cancellationToken);

        return eventTypeExist;
    }
}
