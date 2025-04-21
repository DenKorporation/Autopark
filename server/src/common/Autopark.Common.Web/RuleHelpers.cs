using Microsoft.AspNetCore.Mvc.Filters;

namespace Autopark.Common.Web;

public static class RuleHelpers
{
    public static TActionArgument GetActionArgument<TActionArgument>(
        this ActionExecutingContext context,
        string actionArgumentName)
    {
        if (context.ActionArguments.ContainsKey(actionArgumentName)
            && context.ActionArguments[actionArgumentName] is TActionArgument arg)
        {
            return arg;
        }

        return default;
    }
}
