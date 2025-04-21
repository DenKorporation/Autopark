using Autopark.Common.Attributes;
using Autopark.Common.Bl.Errors;
using Autopark.Dal.Core.Filters;
using Autopark.Dal.Core.Queries;
using Autopark.Dal.Core.Repositories;
using Autopark.PublicApi.Bl.VehicleStatusHistories.Errors;
using Autopark.PublicApi.Bl.VehicleStatusHistories.Services.Interfaces;
using Autopark.PublicApi.Models.VehicleStatusHistories;
using Autopark.PublicApi.Shared.VehicleStatusHistories.Dto;
using FluentResults;

namespace Autopark.PublicApi.Bl.VehicleStatusHistories.Services;

[ServiceAsInterfaces]
public class VehicleStatusHistoryService(
    IEditDtoRepository<VehicleStatusHistory, VehicleStatusHistoryRequest> vehicleStatusHistoryEditDtoRepository,
    IDtoRepository<VehicleStatusHistory, VehicleStatusHistoryResponse> vehicleStatusHistoryDtoRepository) : IVehicleStatusHistoryService
{
    public async Task<Result<QueryResultDto<VehicleStatusHistoryResponse>>> GetAllAsync(QueryFilter<VehicleStatusHistoryFilterDto> filter, CancellationToken cancellationToken = default)
    {
        return await vehicleStatusHistoryDtoRepository.QueryDtoAsync(filter, cancellationToken);
    }

    public async Task<Result<VehicleStatusHistoryResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var vehicleStatusHistoryDto = await vehicleStatusHistoryDtoRepository.GetDtoAsync(id, cancellationToken);

        if (vehicleStatusHistoryDto is null)
        {
            return new VehicleStatusHistoryNotFoundError(id);
        }

        return vehicleStatusHistoryDto;
    }

    public async Task<Result<VehicleStatusHistoryResponse>> CreateVehicleStatusHistoryAsync(VehicleStatusHistoryRequest vehicleStatusHistoryRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            var id = await vehicleStatusHistoryEditDtoRepository.SaveDtoAsync(vehicleStatusHistoryRequest, cancellationToken: cancellationToken);

            return (await vehicleStatusHistoryDtoRepository.GetDtoAsync(id, cancellationToken))!;
        }
        catch (Exception)
        {
            return new InternalServerError("VehicleStatusHistory.Create");
        }
    }

    public async Task<Result<VehicleStatusHistoryResponse>> UpdateVehicleStatusHistoryAsync(Guid id, VehicleStatusHistoryRequest vehicleStatusHistoryRequest, CancellationToken cancellationToken = default)
    {
        var vehicleStatusHistoryDto = await vehicleStatusHistoryDtoRepository.GetDtoAsync(id, cancellationToken);
        if (vehicleStatusHistoryDto is null)
        {
            return new VehicleStatusHistoryNotFoundError(id);
        }

        try
        {
            var updatedId = await vehicleStatusHistoryEditDtoRepository.SaveDtoAsync(vehicleStatusHistoryRequest, cancellationToken: cancellationToken);

            return (await vehicleStatusHistoryDtoRepository.GetDtoAsync(id, cancellationToken))!;
        }
        catch (Exception)
        {
            return new InternalServerError("VehicleStatusHistory.Update");
        }
    }

    public async Task<Result> DeleteVehicleStatusHistoryAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var vehicleStatusHistory = await vehicleStatusHistoryDtoRepository.GetDtoAsync(id, cancellationToken);

        if (vehicleStatusHistory is null)
        {
            return new VehicleStatusHistoryNotFoundError(id);
        }

        try
        {
            await vehicleStatusHistoryDtoRepository.DeleteByIdAsync(id);
        }
        catch (Exception)
        {
            return new InternalServerError("VehicleStatusHistory.Delete");
        }

        return Result.Ok();
    }
}
