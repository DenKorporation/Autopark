using FluentResults;

namespace Autopark.PublicApi.Bl.Authorizations.Services.Interfaces;

public interface IAuthorizationService
{
    Task<Result<Guid>> InitGroupAsync(string email, CancellationToken cancellationToken);
    Task SelectGroupAsync(Guid groupId, CancellationToken cancellationToken);
}
