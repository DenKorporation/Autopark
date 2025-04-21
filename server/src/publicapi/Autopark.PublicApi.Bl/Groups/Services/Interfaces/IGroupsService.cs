using Autopark.PublicApi.Shared.Groups.Dto;
using FluentResults;

namespace Autopark.PublicApi.Bl.Groups.Services.Interfaces;

public interface IGroupsService
{
    Task<Result<IList<GroupResponse>>> GetAllUserGroupsAsync(Guid userId, CancellationToken cancellationToken = default);
}
