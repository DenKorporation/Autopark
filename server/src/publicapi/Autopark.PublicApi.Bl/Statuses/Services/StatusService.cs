using Autopark.Common.Attributes;
using Autopark.Common.Bl.Errors;
using Autopark.Dal.Core.Filters;
using Autopark.Dal.Core.Queries;
using Autopark.Dal.Core.Repositories;
using Autopark.PublicApi.Bl.Statuses.Errors;
using Autopark.PublicApi.Bl.Statuses.Services.Interfaces;
using Autopark.PublicApi.Models.Statuses;
using Autopark.PublicApi.Shared.Statuses.Dto;
using FluentResults;

namespace Autopark.PublicApi.Bl.Statuses.Services;

[ServiceAsInterfaces]
public class StatusService(
    IEditDtoRepository<Status, StatusRequest> statusEditDtoRepository,
    IDtoRepository<Status, StatusResponse> statusDtoRepository) : IStatusService
{
    public async Task<Result<QueryResultDto<StatusResponse>>> GetAllAsync(QueryFilter<StatusFilterDto> filter, CancellationToken cancellationToken = default)
    {
        return await statusDtoRepository.QueryDtoAsync(filter, cancellationToken);
    }

    public async Task<Result<StatusResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var statusDto = await statusDtoRepository.GetDtoAsync(id, cancellationToken);

        if (statusDto is null)
        {
            return new StatusNotFoundError(id);
        }

        return statusDto;
    }

    public async Task<Result<StatusResponse>> CreateStatusAsync(StatusRequest statusRequest, CancellationToken cancellationToken = default)
    {
        var result = await EnsureStatusByNameNotExistAsync(statusRequest.Name, cancellationToken);
        if (result.IsFailed)
        {
            return result;
        }

        try
        {
            var id = await statusEditDtoRepository.SaveDtoAsync(statusRequest, cancellationToken: cancellationToken);

            return (await statusDtoRepository.GetDtoAsync(id, cancellationToken))!;
        }
        catch (Exception)
        {
            return new InternalServerError("Status.Create");
        }
    }

    public async Task<Result<StatusResponse>> UpdateStatusAsync(Guid id, StatusRequest statusRequest, CancellationToken cancellationToken = default)
    {
        var statusDto = await statusDtoRepository.GetDtoAsync(id, cancellationToken);
        if (statusDto is null)
        {
            return new StatusNotFoundError(id);
        }

        if (statusDto.Name != statusRequest.Name)
        {
            var result = await EnsureStatusByNameNotExistAsync(statusRequest.Name, cancellationToken);
            if (result.IsFailed)
            {
                return result;
            }    
        }

        try
        {
            var updatedId = await statusEditDtoRepository.SaveDtoAsync(statusRequest, cancellationToken: cancellationToken);

            return (await statusDtoRepository.GetDtoAsync(id, cancellationToken))!;
        }
        catch (Exception)
        {
            return new InternalServerError("Status.Update");
        }
    }

    public async Task<Result> DeleteStatusAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var status = await statusDtoRepository.GetDtoAsync(id, cancellationToken);

        if (status is null)
        {
            return new StatusNotFoundError(id);
        }

        try
        {
            await statusDtoRepository.DeleteByIdAsync(id);
        }
        catch (Exception)
        {
            return new InternalServerError("Status.Delete");
        }

        return Result.Ok();
    }

    private async Task<Result> EnsureStatusByNameNotExistAsync(string name, CancellationToken cancellationToken = default)
    {
        var statusExist = await statusEditDtoRepository.AnyByFilterAsync(
            new StatusFilterDto()
            {
                Name = name,
            }, cancellationToken);

        if (statusExist)
        {
            return new StatusDuplicationError(name);
        }

        return Result.Ok();
    }
}
