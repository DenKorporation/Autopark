using System.Security.Claims;
using Autofac.Features.Metadata;
using Autopark.Common.Security;

namespace Autopark.Common.Web.Security;

public class CompositeUserInfoProvider : IUserInfoProvider
{
    private readonly IReadOnlyList<IUserInfoProvider> userInfoProviders;

    public CompositeUserInfoProvider(
        IEnumerable<Meta<IUserInfoProvider>> userInfoProviders)
    {
        this.userInfoProviders = userInfoProviders
            .OrderBy(f => f.Metadata.ContainsKey("Order") ? f.Metadata["Order"] : -1)
            .Select(s => s.Value).ToArray();
    }

    /// <inheritdoc />
    public bool IsAccessible => userInfoProviders.Any(x => x.IsAccessible);

    public Guid? GetUserId()
    {
        return Provider.GetUserId();
    }

    private IUserInfoProvider Provider => userInfoProviders.First(x => x.IsAccessible);

    public Claim GetClaim(string claim)
    {
        return Provider.GetClaim(claim);
    }

    public Guid GetGroupId()
    {
        return Provider.GetGroupId();
    }

    public IEnumerable<Claim> GetClaims(string claim)
    {
        return Provider.GetClaims(claim);
    }

    public string GetCurrentUserLogin()
    {
        return Provider.GetCurrentUserLogin();
    }

    public string? GetGroupName()
    {
        return Provider.GetGroupName();
    }

    public bool HasClaim(string type, string value)
    {
        return Provider.HasClaim(type, value);
    }

    public bool IsAuthenticated()
    {
        return Provider.IsAuthenticated();
    }

    /// <inheritdoc />
    public string GetAuthorizationData()
    {
        return Provider.GetAuthorizationData();
    }
}