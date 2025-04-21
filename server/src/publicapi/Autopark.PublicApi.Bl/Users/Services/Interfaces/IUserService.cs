using Autopark.Dal.Core.Filters;
using Autopark.Dal.Core.Queries;
using Autopark.PublicApi.Shared.Users.Dto;
using FluentResults;

namespace Autopark.PublicApi.Bl.Users.Services.Interfaces;

public interface IUserService
{
    Task<Result<QueryResultDto<UserResponse>>> GetAllAsync(QueryFilter<UserFilterDto> filter, CancellationToken cancellationToken = default);

    Task<Result<UserResponse>> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<Result<UserResponse>> CreateUserAsync(
        UserRequest userRequest,
        CancellationToken cancellationToken = default);
}
