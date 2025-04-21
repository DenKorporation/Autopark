using Autopark.Dal.Core.Filters;
using Autopark.Dal.Core.Queries;
using Autopark.PublicApi.Shared.Parts.Dto;
using FluentResults;

namespace Autopark.PublicApi.Bl.Parts.Services.Interfaces;

public interface IPartService
{
    Task<Result<QueryResultDto<PartResponse>>> GetAllAsync(QueryFilter<PartFilterDto> filter, CancellationToken cancellationToken = default);

    Task<Result<PartResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result<PartResponse>> CreatePartAsync(
        PartRequest partRequest,
        CancellationToken cancellationToken = default);

    Task<Result<PartResponse>> UpdatePartAsync(
        Guid id,
        PartRequest partRequest,
        CancellationToken cancellationToken = default);

    Task<Result> DeletePartAsync(Guid id, CancellationToken cancellationToken = default);
}
