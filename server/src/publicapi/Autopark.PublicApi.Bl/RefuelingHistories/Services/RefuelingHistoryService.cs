using Autopark.Common.Attributes;
using Autopark.Common.Bl.Errors;
using Autopark.Dal.Core.Filters;
using Autopark.Dal.Core.Queries;
using Autopark.Dal.Core.Repositories;
using Autopark.PublicApi.Bl.RefuelingHistories.Errors;
using Autopark.PublicApi.Bl.RefuelingHistories.Services.Interfaces;
using Autopark.PublicApi.Models.RefuelingHistories;
using Autopark.PublicApi.Shared.RefuelingHistories.Dto;
using FluentResults;

namespace Autopark.PublicApi.Bl.RefuelingHistories.Services;

[ServiceAsInterfaces]
public class RefuelingHistoryService(
    IEditDtoRepository<RefuelingHistory, RefuelingHistoryRequest> refuelingHistoryEditDtoRepository,
    IDtoRepository<RefuelingHistory, RefuelingHistoryResponse> refuelingHistoryDtoRepository) : IRefuelingHistoryService
{
    public async Task<Result<QueryResultDto<RefuelingHistoryResponse>>> GetAllAsync(QueryFilter<RefuelingHistoryFilterDto> filter, CancellationToken cancellationToken = default)
    {
        return await refuelingHistoryDtoRepository.QueryDtoAsync(filter, cancellationToken);
    }

    public async Task<Result<RefuelingHistoryResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var refuelingHistoryDto = await refuelingHistoryDtoRepository.GetDtoAsync(id, cancellationToken);

        if (refuelingHistoryDto is null)
        {
            return new RefuelingHistoryNotFoundError(id);
        }

        return refuelingHistoryDto;
    }

    public async Task<Result<RefuelingHistoryResponse>> CreateRefuelingHistoryAsync(RefuelingHistoryRequest refuelingHistoryRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            var id = await refuelingHistoryEditDtoRepository.SaveDtoAsync(refuelingHistoryRequest, cancellationToken: cancellationToken);

            return (await refuelingHistoryDtoRepository.GetDtoAsync(id, cancellationToken))!;
        }
        catch (Exception)
        {
            return new InternalServerError("RefuelingHistory.Create");
        }
    }

    public async Task<Result<RefuelingHistoryResponse>> UpdateRefuelingHistoryAsync(Guid id, RefuelingHistoryRequest refuelingHistoryRequest, CancellationToken cancellationToken = default)
    {
        var refuelingHistoryDto = await refuelingHistoryDtoRepository.GetDtoAsync(id, cancellationToken);
        if (refuelingHistoryDto is null)
        {
            return new RefuelingHistoryNotFoundError(id);
        }

        try
        {
            var updatedId = await refuelingHistoryEditDtoRepository.SaveDtoAsync(refuelingHistoryRequest, cancellationToken: cancellationToken);

            return (await refuelingHistoryDtoRepository.GetDtoAsync(id, cancellationToken))!;
        }
        catch (Exception)
        {
            return new InternalServerError("RefuelingHistory.Update");
        }
    }

    public async Task<Result> DeleteRefuelingHistoryAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var refuelingHistory = await refuelingHistoryDtoRepository.GetDtoAsync(id, cancellationToken);

        if (refuelingHistory is null)
        {
            return new RefuelingHistoryNotFoundError(id);
        }

        try
        {
            await refuelingHistoryDtoRepository.DeleteByIdAsync(id);
        }
        catch (Exception)
        {
            return new InternalServerError("RefuelingHistory.Delete");
        }

        return Result.Ok();
    }
}
