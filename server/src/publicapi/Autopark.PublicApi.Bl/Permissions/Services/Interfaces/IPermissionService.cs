using Autopark.Dal.Core.Filters;
using Autopark.Dal.Core.Queries;
using Autopark.PublicApi.Shared.Permissions.Dto;
using FluentResults;

namespace Autopark.PublicApi.Bl.Permissions.Services.Interfaces;

public interface IPermissionService
{
    Task<Result<QueryResultDto<PermissionResponse>>> GetAllAsync(QueryFilter<PermissionFilterDto> filter, CancellationToken cancellationToken = default);

    Task<Result<PermissionResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result<PermissionResponse>> CreatePermissionAsync(
        PermissionRequest permissionRequest,
        CancellationToken cancellationToken = default);

    Task<Result<PermissionResponse>> UpdatePermissionAsync(
        Guid id,
        PermissionRequest permissionRequest,
        CancellationToken cancellationToken = default);

    Task<Result> DeletePermissionAsync(Guid id, CancellationToken cancellationToken = default);
}
