using Autopark.Dal.Core.Filters;
using Autopark.Dal.Core.Queries;
using Autopark.PublicApi.Shared.Statuses.Dto;
using FluentResults;

namespace Autopark.PublicApi.Bl.Statuses.Services.Interfaces;

public interface IStatusService
{
    Task<Result<QueryResultDto<StatusResponse>>> GetAllAsync(QueryFilter<StatusFilterDto> filter, CancellationToken cancellationToken = default);

    Task<Result<StatusResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result<StatusResponse>> CreateStatusAsync(
        StatusRequest statusRequest,
        CancellationToken cancellationToken = default);

    Task<Result<StatusResponse>> UpdateStatusAsync(
        Guid id,
        StatusRequest statusRequest,
        CancellationToken cancellationToken = default);

    Task<Result> DeleteStatusAsync(Guid id, CancellationToken cancellationToken = default);
}
