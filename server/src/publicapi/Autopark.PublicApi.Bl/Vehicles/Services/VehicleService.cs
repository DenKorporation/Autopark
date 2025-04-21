using Autopark.Common.Attributes;
using Autopark.Common.Bl.Errors;
using Autopark.Dal.Core.Filters;
using Autopark.Dal.Core.Queries;
using Autopark.Dal.Core.Repositories;
using Autopark.PublicApi.Bl.Vehicles.Errors;
using Autopark.PublicApi.Bl.Vehicles.Services.Interfaces;
using Autopark.PublicApi.Models.Vehicles;
using Autopark.PublicApi.Shared.Vehicles.Dto;
using FluentResults;

namespace Autopark.PublicApi.Bl.Vehicles.Services;

[ServiceAsInterfaces]
public class VehicleService(
    IEditDtoRepository<Vehicle, VehicleRequest> vehicleEditDtoRepository,
    IDtoRepository<Vehicle, VehicleResponse> vehicleDtoRepository) : IVehicleService
{
    public async Task<Result<QueryResultDto<VehicleResponse>>> GetAllAsync(QueryFilter<VehicleFilterDto> filter, CancellationToken cancellationToken = default)
    {
        return await vehicleDtoRepository.QueryDtoAsync(filter, cancellationToken);
    }

    public async Task<Result<VehicleResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var vehicleDto = await vehicleDtoRepository.GetDtoAsync(id, cancellationToken);

        if (vehicleDto is null)
        {
            return new VehicleNotFoundError(id);
        }

        return vehicleDto;
    }

    public async Task<Result<VehicleResponse>> CreateVehicleAsync(VehicleRequest vehicleRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            var id = await vehicleEditDtoRepository.SaveDtoAsync(vehicleRequest, cancellationToken: cancellationToken);

            return (await vehicleDtoRepository.GetDtoAsync(id, cancellationToken))!;
        }
        catch (Exception)
        {
            return new InternalServerError("Vehicle.Create");
        }
    }

    public async Task<Result<VehicleResponse>> UpdateVehicleAsync(Guid id, VehicleRequest vehicleRequest, CancellationToken cancellationToken = default)
    {
        var vehicleDto = await vehicleDtoRepository.GetDtoAsync(id, cancellationToken);
        if (vehicleDto is null)
        {
            return new VehicleNotFoundError(id);
        }

        try
        {
            var updatedId = await vehicleEditDtoRepository.SaveDtoAsync(vehicleRequest, cancellationToken: cancellationToken);

            return (await vehicleDtoRepository.GetDtoAsync(id, cancellationToken))!;
        }
        catch (Exception)
        {
            return new InternalServerError("Vehicle.Update");
        }
    }

    public async Task<Result> DeleteVehicleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var vehicle = await vehicleDtoRepository.GetDtoAsync(id, cancellationToken);

        if (vehicle is null)
        {
            return new VehicleNotFoundError(id);
        }

        try
        {
            await vehicleDtoRepository.DeleteByIdAsync(id);
        }
        catch (Exception)
        {
            return new InternalServerError("Vehicle.Delete");
        }

        return Result.Ok();
    }
}
