using System.Security.Claims;
using Autopark.Common.Attributes;
using Autopark.Common.Constants;
using Autopark.Common.Security;
using Microsoft.AspNetCore.Http;

namespace Autopark.Common.Web.Security;

[ServiceAsInterfaces(Order = 0)]
public class UserInfoProvider : IUserInfoProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserInfoProvider(IHttpContextAccessor httpContextAccessor)
    {
        this._httpContextAccessor = httpContextAccessor;
    }

    /// <inheritdoc />
    public bool IsAccessible => _httpContextAccessor.HttpContext != null;

    /// <inheritdoc />
    public string GetAuthorizationData()
    {
        return _httpContextAccessor?.HttpContext?.Request?.Headers?.Authorization;
    }

    public bool IsAuthenticated()
    {
        return _httpContextAccessor.HttpContext?.User?.Identity.IsAuthenticated == true;
    }

    public Guid? GetUserId()
    {
        var claim = GetClaim(ClaimTypes.NameIdentifier);

        if (claim == null)
        {
            return null;
        }

        if (!Guid.TryParse(claim.Value, out var id) || id == Guid.Empty)
        {
            return null;
        }

        return id;
    }

    public string GetCurrentUserLogin()
    {
        return _httpContextAccessor.HttpContext?.User?.Identity?.Name;
    }

    public string? GetGroupName()
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            return null;
        }

        _httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(CookieKeys.GroupName, out var value);

        return value;
    }

    public Guid GetGroupId()
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            return default(Guid);
        }

        _httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(CookieKeys.GroupId, out var value);

        Guid.TryParse(value, out var guidValue);

        return guidValue;
    }

    public IEnumerable<Claim> GetClaims(string claim)
    {
        return _httpContextAccessor.HttpContext?.User?.FindAll(claim);
    }

    public Claim GetClaim(string claim)
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirst(claim);
    }

    public bool HasClaim(string type, string value)
    {
        return _httpContextAccessor.HttpContext?.User?.HasClaim(type, value) == true;
    }
}
