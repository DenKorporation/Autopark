using Autopark.Common.Attributes;
using Autopark.Common.Bl.Errors;
using Autopark.Dal.Core.Filters;
using Autopark.Dal.Core.Queries;
using Autopark.Dal.Core.Repositories;
using Autopark.PublicApi.Bl.Permissions.Errors;
using Autopark.PublicApi.Bl.Permissions.Services.Interfaces;
using Autopark.PublicApi.Models.Permissions;
using Autopark.PublicApi.Shared.Permissions.Dto;
using FluentResults;

namespace Autopark.PublicApi.Bl.Permissions.Services;

[ServiceAsInterfaces]
public class PermissionService(
    IEditDtoRepository<Permission, PermissionRequest> permissionEditDtoRepository,
    IDtoRepository<Permission, PermissionResponse> permissionDtoRepository) : IPermissionService
{
    public async Task<Result<QueryResultDto<PermissionResponse>>> GetAllAsync(QueryFilter<PermissionFilterDto> filter, CancellationToken cancellationToken = default)
    {
        return await permissionDtoRepository.QueryDtoAsync(filter, cancellationToken);
    }

    public async Task<Result<PermissionResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var permissionDto = await permissionDtoRepository.GetDtoAsync(id, cancellationToken);

        if (permissionDto is null)
        {
            return new PermissionNotFoundError(id);
        }

        return permissionDto;
    }

    public async Task<Result<PermissionResponse>> CreatePermissionAsync(PermissionRequest permissionRequest, CancellationToken cancellationToken = default)
    {
        var result = await EnsurePermissionForVehicleNotExist(permissionRequest.VehicleId, cancellationToken);
        if (result.IsFailed)
        {
            return result;
        }

        result = await EnsurePermissionForNumberNotExist(permissionRequest.Number, cancellationToken);
        if (result.IsFailed)
        {
            return result;
        }

        try
        {
            var id = await permissionEditDtoRepository.SaveDtoAsync(permissionRequest, cancellationToken: cancellationToken);

            return (await permissionDtoRepository.GetDtoAsync(id, cancellationToken))!;
        }
        catch (Exception)
        {
            return new InternalServerError("Permission.Create");
        }
    }

    public async Task<Result<PermissionResponse>> UpdatePermissionAsync(Guid id, PermissionRequest permissionRequest, CancellationToken cancellationToken = default)
    {
        var permissionDto = await permissionDtoRepository.GetDtoAsync(id, cancellationToken);
        if (permissionDto is null)
        {
            return new PermissionNotFoundError(id);
        }

        if (permissionRequest.Number != permissionDto.Number)
        {
            var result = await EnsurePermissionForNumberNotExist(permissionRequest.Number, cancellationToken);
            if (result.IsFailed)
            {
                return result;
            }
        }

        if (permissionRequest.VehicleId != permissionDto.VehicleId)
        {
            var result = await EnsurePermissionForVehicleNotExist(permissionRequest.VehicleId, cancellationToken);
            if (result.IsFailed)
            {
                return result;
            }
        }

        try
        {
            var updatedId = await permissionEditDtoRepository.SaveDtoAsync(permissionRequest, cancellationToken: cancellationToken);

            return (await permissionDtoRepository.GetDtoAsync(id, cancellationToken))!;
        }
        catch (Exception)
        {
            return new InternalServerError("Permission.Update");
        }
    }

    public async Task<Result> DeletePermissionAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var permission = await permissionDtoRepository.GetDtoAsync(id, cancellationToken);
        if (permission is null)
        {
            return new PermissionNotFoundError(id);
        }

        try
        {
            await permissionDtoRepository.DeleteByIdAsync(id);
        }
        catch (Exception)
        {
            return new InternalServerError("Permission.Delete");
        }

        return Result.Ok();
    }

    private async Task<Result> EnsurePermissionForVehicleNotExist(Guid vehicleId, CancellationToken cancellationToken)
    {
        var permissionForVehicleExist = await permissionDtoRepository.AnyByFilterAsync(
            new PermissionFilterDto()
            {
                VehicleId = vehicleId 
            }, cancellationToken);

        if (permissionForVehicleExist)
        {
            return new PermissionDuplicationError(vehicleId);
        }

        return Result.Ok();
    }

    private async Task<Result> EnsurePermissionForNumberNotExist(string number, CancellationToken cancellationToken)
    {
        var permissionExist = await permissionDtoRepository.AnyByFilterAsync(
            new PermissionFilterDto()
            {
                Number = number 
            }, cancellationToken);

        if (permissionExist)
        {
            return new PermissionDuplicationError(number);
        }

        return Result.Ok();
    }
}
