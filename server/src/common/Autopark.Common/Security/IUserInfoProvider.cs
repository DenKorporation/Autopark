using System.Security.Claims;

namespace Autopark.Common.Security;

public interface IUserInfoProvider
{
    bool IsAccessible { get; }

    Guid? GetUserId();

    string GetCurrentUserLogin();

    string? GetGroupName();

    Guid GetGroupId();

    IEnumerable<Claim> GetClaims(string claim);

    bool HasClaim(string type, string value);

    bool IsAuthenticated();

    Claim GetClaim(string claim);

    string GetAuthorizationData();
}