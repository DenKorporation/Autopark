using Autopark.Dal.Core.Filters;
using Autopark.Dal.Core.Queries;
using Autopark.PublicApi.Shared.PartReplacements.Dto;
using FluentResults;

namespace Autopark.PublicApi.Bl.PartReplacements.Services.Interfaces;

public interface IPartReplacementService
{
    Task<Result<QueryResultDto<PartReplacementResponse>>> GetAllAsync(QueryFilter<PartReplacementFilterDto> filter, CancellationToken cancellationToken = default);

    Task<Result<PartReplacementResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result<PartReplacementResponse>> CreatePartReplacementAsync(
        PartReplacementRequest partReplacementRequest,
        CancellationToken cancellationToken = default);

    Task<Result<PartReplacementResponse>> UpdatePartReplacementAsync(
        Guid id,
        PartReplacementRequest partReplacementRequest,
        CancellationToken cancellationToken = default);

    Task<Result> DeletePartReplacementAsync(Guid id, CancellationToken cancellationToken = default);
}
