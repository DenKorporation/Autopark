using Microsoft.AspNetCore.Mvc.Filters;

namespace Autopark.Common.Web.Security;

public interface IPermissionRule
{
    Task<bool> AllowedAsync(ActionExecutingContext context);
}