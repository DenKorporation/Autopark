using Autopark.Common.Attributes;
using Autopark.Common.Bl.Errors;
using Autopark.Dal.Core.Filters;
using Autopark.Dal.Core.Queries;
using Autopark.Dal.Core.Repositories;
using Autopark.PublicApi.Bl.OdometerHistories.Errors;
using Autopark.PublicApi.Bl.OdometerHistories.Services.Interfaces;
using Autopark.PublicApi.Models.OdometerHistories;
using Autopark.PublicApi.Shared.OdometerHistories.Dto;
using FluentResults;

namespace Autopark.PublicApi.Bl.OdometerHistories.Services;

[ServiceAsInterfaces]
public class OdometerHistoryService(
    IEditDtoRepository<OdometerHistory, OdometerHistoryRequest> odometerHistoryEditDtoRepository,
    IDtoRepository<OdometerHistory, OdometerHistoryResponse> odometerHistoryDtoRepository) : IOdometerHistoryService
{
    public async Task<Result<QueryResultDto<OdometerHistoryResponse>>> GetAllAsync(QueryFilter<OdometerHistoryFilterDto> filter, CancellationToken cancellationToken = default)
    {
        return await odometerHistoryDtoRepository.QueryDtoAsync(filter, cancellationToken);
    }

    public async Task<Result<OdometerHistoryResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var odometerHistoryDto = await odometerHistoryDtoRepository.GetDtoAsync(id, cancellationToken);

        if (odometerHistoryDto is null)
        {
            return new OdometerHistoryNotFoundError(id);
        }

        return odometerHistoryDto;
    }

    public async Task<Result<OdometerHistoryResponse>> CreateOdometerHistoryAsync(OdometerHistoryRequest odometerHistoryRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            var id = await odometerHistoryEditDtoRepository.SaveDtoAsync(odometerHistoryRequest, cancellationToken: cancellationToken);

            return (await odometerHistoryDtoRepository.GetDtoAsync(id, cancellationToken))!;
        }
        catch (Exception)
        {
            return new InternalServerError("OdometerHistory.Create");
        }
    }

    public async Task<Result<OdometerHistoryResponse>> UpdateOdometerHistoryAsync(Guid id, OdometerHistoryRequest odometerHistoryRequest, CancellationToken cancellationToken = default)
    {
        var odometerHistoryDto = await odometerHistoryDtoRepository.GetDtoAsync(id, cancellationToken);
        if (odometerHistoryDto is null)
        {
            return new OdometerHistoryNotFoundError(id);
        }

        try
        {
            var updatedId = await odometerHistoryEditDtoRepository.SaveDtoAsync(odometerHistoryRequest, cancellationToken: cancellationToken);

            return (await odometerHistoryDtoRepository.GetDtoAsync(id, cancellationToken))!;
        }
        catch (Exception)
        {
            return new InternalServerError("OdometerHistory.Update");
        }
    }

    public async Task<Result> DeleteOdometerHistoryAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var odometerHistory = await odometerHistoryDtoRepository.GetDtoAsync(id, cancellationToken);

        if (odometerHistory is null)
        {
            return new OdometerHistoryNotFoundError(id);
        }

        try
        {
            await odometerHistoryDtoRepository.DeleteByIdAsync(id);
        }
        catch (Exception)
        {
            return new InternalServerError("OdometerHistory.Delete");
        }

        return Result.Ok();
    }
}
