using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;
using Autopark.Common.Constants;
using Autopark.Common.Exceptions;
using Autopark.Common.Extensions;
using Autopark.Common.Security;
using Autopark.Common.Web.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Autopark.Common.Web.Filters;

public class PermissionFilter(
    IServiceProvider serviceProvider,
    IUserInfoProvider userInfoProvider,
    ILogger<PermissionFilter> logger,
    IHttpContextAccessor httpContextAccessor)
    : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
        {
            if (!userInfoProvider.IsAuthenticated())
            {
                var allowAnonymous = GetActionAttribute<AllowAnonymousAttribute>(controllerActionDescriptor);
                if (allowAnonymous == null)
                {
                    logger.LogDebug(
                        $"User not authorized to request action {context.ActionDescriptor.DisplayName}");

                    throw new UnauthorizedException();
                }
            }
            else
            {
                if (controllerActionDescriptor.ControllerName != "Authorization" ||
                    (controllerActionDescriptor.ActionName != "SelectGroup" && controllerActionDescriptor.ActionName != "Logout"))
                {
                    var cookies = context.HttpContext.Request.Cookies;

                    if (!cookies.ContainsKey(CookieKeys.GroupId) ||
                        !Guid.TryParse(cookies[CookieKeys.GroupId], out _) ||
                        !cookies.ContainsKey(CookieKeys.GroupName) ||
                        cookies[CookieKeys.GroupName].IsNullOrEmpty())
                    {
                        logger.LogDebug(
                            "Missing cookies for user {0}",
                            userInfoProvider.GetCurrentUserLogin());

                        throw new BadRequestException($"Missing cookie: {CookieKeys.GroupId}, {CookieKeys.GroupName}");
                    }
                }
            }

            var permissionAttribute = GetActionAttribute<PermissionAttribute>(controllerActionDescriptor);
            if (permissionAttribute != null)
            {
                if (!await HasAnyAllowedRule(permissionAttribute.RuleTypes, context) &&
                    !await HasAnyRole(permissionAttribute.Roles, context))
                {
                    logger.LogDebug(
                        "User {0} is not allowed to request action {1}, claims: {2}",
                        userInfoProvider.GetCurrentUserLogin(),
                        context.ActionDescriptor.DisplayName,
                        userInfoProvider.GetClaims(ClaimTypes.Role));

                    throw new ForbiddenException();
                }
            }
        }

        logger.LogDebug($"User allowed to request action {context.ActionDescriptor.DisplayName}");
        await next();
    }

    private async Task<bool> HasAnyAllowedRule(Type[] ruleTypes, ActionExecutingContext context)
    {
        if (ruleTypes == null)
        {
            return false;
        }

        foreach (var ruleType in ruleTypes)
        {
            var rule = (IPermissionRule)serviceProvider.GetRequiredService(ruleType);

            if (await rule.AllowedAsync(context))
            {
                return true;
            }
        }

        return false;
    }
    
    private async Task<bool> HasAnyRole(string? roles, ActionExecutingContext context)
    {
        if (roles is null)
        {
            return false;
        }
        
        var roleList = roles.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        foreach (var role in roleList)
        {
            if (httpContextAccessor.HttpContext.User.IsInRole(role))
            {
                return true;
            }
        }

        return false;
    }

    private TAttribute GetActionAttribute<TAttribute>(ControllerActionDescriptor controllerActionDescriptor)
        where TAttribute : Attribute
    {
        return controllerActionDescriptor.MethodInfo.GetCustomAttribute<TAttribute>()
               ?? controllerActionDescriptor.MethodInfo.DeclaringType?.GetCustomAttribute<TAttribute>();
    }
}
