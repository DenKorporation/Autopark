using Autopark.Common.Attributes;
using Autopark.Common.Bl.Errors;
using Autopark.Dal.Core.Filters;
using Autopark.Dal.Core.Queries;
using Autopark.Dal.Core.Repositories;
using Autopark.PublicApi.Bl.Parts.Errors;
using Autopark.PublicApi.Bl.Parts.Services.Interfaces;
using Autopark.PublicApi.Models.Parts;
using Autopark.PublicApi.Shared.Parts.Dto;
using FluentResults;

namespace Autopark.PublicApi.Bl.Parts.Services;

[ServiceAsInterfaces]
public class PartService(
    IEditDtoRepository<Part, PartRequest> partEditDtoRepository,
    IDtoRepository<Part, PartResponse> partDtoRepository) : IPartService
{
    public async Task<Result<QueryResultDto<PartResponse>>> GetAllAsync(QueryFilter<PartFilterDto> filter, CancellationToken cancellationToken = default)
    {
        return await partDtoRepository.QueryDtoAsync(filter, cancellationToken);
    }

    public async Task<Result<PartResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var partDto = await partDtoRepository.GetDtoAsync(id, cancellationToken);

        if (partDto is null)
        {
            return new PartNotFoundError(id);
        }

        return partDto;
    }

    public async Task<Result<PartResponse>> CreatePartAsync(PartRequest partRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            var id = await partEditDtoRepository.SaveDtoAsync(partRequest, cancellationToken: cancellationToken);

            return (await partDtoRepository.GetDtoAsync(id, cancellationToken))!;
        }
        catch (Exception)
        {
            return new InternalServerError("Part.Create");
        }
    }

    public async Task<Result<PartResponse>> UpdatePartAsync(Guid id, PartRequest partRequest, CancellationToken cancellationToken = default)
    {
        var partDto = await partDtoRepository.GetDtoAsync(id, cancellationToken);
        if (partDto is null)
        {
            return new PartNotFoundError(id);
        }

        try
        {
            var updatedId = await partEditDtoRepository.SaveDtoAsync(partRequest, cancellationToken: cancellationToken);

            return (await partDtoRepository.GetDtoAsync(id, cancellationToken))!;
        }
        catch (Exception)
        {
            return new InternalServerError("Part.Update");
        }
    }

    public async Task<Result> DeletePartAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var part = await partDtoRepository.GetDtoAsync(id, cancellationToken);

        if (part is null)
        {
            return new PartNotFoundError(id);
        }

        try
        {
            await partDtoRepository.DeleteByIdAsync(id);
        }
        catch (Exception)
        {
            return new InternalServerError("Part.Delete");
        }

        return Result.Ok();
    }
}
