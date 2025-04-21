using Autopark.Common.Attributes;
using Autopark.Common.Bl.Errors;
using Autopark.Dal.Core.Filters;
using Autopark.Dal.Core.Queries;
using Autopark.Dal.Core.Repositories;
using Autopark.PublicApi.Bl.PartReplacements.Errors;
using Autopark.PublicApi.Bl.PartReplacements.Services.Interfaces;
using Autopark.PublicApi.Models.PartReplacements;
using Autopark.PublicApi.Shared.PartReplacements.Dto;
using FluentResults;

namespace Autopark.PublicApi.Bl.PartReplacements.Services;

[ServiceAsInterfaces]
public class PartReplacementService(
    IEditDtoRepository<PartReplacement, PartReplacementRequest> partReplacementEditDtoRepository,
    IDtoRepository<PartReplacement, PartReplacementResponse> partReplacementDtoRepository) : IPartReplacementService
{
    public async Task<Result<QueryResultDto<PartReplacementResponse>>> GetAllAsync(QueryFilter<PartReplacementFilterDto> filter, CancellationToken cancellationToken = default)
    {
        return await partReplacementDtoRepository.QueryDtoAsync(filter, cancellationToken);
    }

    public async Task<Result<PartReplacementResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var partReplacementDto = await partReplacementDtoRepository.GetDtoAsync(id, cancellationToken);

        if (partReplacementDto is null)
        {
            return new PartReplacementNotFoundError(id);
        }

        return partReplacementDto;
    }

    public async Task<Result<PartReplacementResponse>> CreatePartReplacementAsync(PartReplacementRequest partReplacementRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            var id = await partReplacementEditDtoRepository.SaveDtoAsync(partReplacementRequest, cancellationToken: cancellationToken);

            return (await partReplacementDtoRepository.GetDtoAsync(id, cancellationToken))!;
        }
        catch (Exception)
        {
            return new InternalServerError("PartReplacement.Create");
        }
    }

    public async Task<Result<PartReplacementResponse>> UpdatePartReplacementAsync(Guid id, PartReplacementRequest partReplacementRequest, CancellationToken cancellationToken = default)
    {
        var partReplacementDto = await partReplacementDtoRepository.GetDtoAsync(id, cancellationToken);
        if (partReplacementDto is null)
        {
            return new PartReplacementNotFoundError(id);
        }

        try
        {
            var updatedId = await partReplacementEditDtoRepository.SaveDtoAsync(partReplacementRequest, cancellationToken: cancellationToken);

            return (await partReplacementDtoRepository.GetDtoAsync(id, cancellationToken))!;
        }
        catch (Exception)
        {
            return new InternalServerError("PartReplacement.Update");
        }
    }

    public async Task<Result> DeletePartReplacementAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var partReplacement = await partReplacementDtoRepository.GetDtoAsync(id, cancellationToken);

        if (partReplacement is null)
        {
            return new PartReplacementNotFoundError(id);
        }

        try
        {
            await partReplacementDtoRepository.DeleteByIdAsync(id);
        }
        catch (Exception)
        {
            return new InternalServerError("PartReplacement.Delete");
        }

        return Result.Ok();
    }
}
