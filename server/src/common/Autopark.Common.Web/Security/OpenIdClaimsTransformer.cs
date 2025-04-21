using System.Security.Claims;
using Autopark.Common.Extensions;
using Autopark.Common.Security;
using Microsoft.AspNetCore.Authentication;

namespace Autopark.Common.Web.Security;

public class OpenIdClaimsTransformer(IUserInfoProvider userInfoProvider) : IClaimsTransformation
{
    private const string RoleClaimFormat = "/{0}/";
    private const string RoleClaim = "role";

    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var claimsIdentity = (ClaimsIdentity)principal.Identity!;
        var allowedRoles = RoleExtensions.GetAllRoles();
        var roles = principal.Claims.Where(c => c.Type == RoleClaim).ToList();

        foreach (var roleClaim in roles)
        {
            claimsIdentity.RemoveClaim(roleClaim);

            var role = GetRoleFromClaim(roleClaim.Value);

            if (role is not null && allowedRoles.Contains(role))
            {
                claimsIdentity.AddClaim(new Claim(claimsIdentity.RoleClaimType, role));
            }
        }

        return Task.FromResult(principal);
    }

    private string? GetRoleFromClaim(string claimValue)
    {
        var groupName = userInfoProvider.GetGroupName();
        var prefix = string.Format(RoleClaimFormat, groupName);

        if (!claimValue.StartsWith(prefix))
        {
            return null;
        }

        return claimValue[prefix.Length..];
    }
}
