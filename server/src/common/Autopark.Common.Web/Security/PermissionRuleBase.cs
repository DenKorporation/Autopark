using Autopark.Common.Security;
using Autopark.Dal.Core.Repositories;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Autopark.Common.Web.Security;

public abstract class PermissionRuleBase(
    IUserInfoProvider userInfoProvider,
    IRepository repository)
    : IPermissionRule
{

    protected IUserInfoProvider UserInfoProvider { get; } = userInfoProvider;

    protected IRepository Repository { get; } = repository;

    public abstract Task<bool> AllowedAsync(ActionExecutingContext context);

    protected virtual TActionArgument GetActionArgument<TActionArgument>(
        ActionExecutingContext context,
        string actionArgumentName)
    {
        return context.GetActionArgument<TActionArgument>(actionArgumentName);
    }
}