using System.Reflection;
using Autopark.Common.Validations;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Autopark.Common.Web.Filters;

public class ValidationFilter(
    IServiceProvider serviceProvider)
    : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var actionArguments = context.ActionArguments;

        var refTypeArguments = actionArguments
            .Where(arg => IsReferenceType(arg.Value.GetType()))
            .ToDictionary(entry => entry.Key, entry => entry.Value);

        foreach (var pair in refTypeArguments)
        {
            var modelType = pair.Value.GetType();
            var genericValidatorType = typeof(IModelValidator<>).MakeGenericType(modelType);
            var validators = serviceProvider.GetServices(genericValidatorType);

            var method = genericValidatorType.GetMethod("ThrowIfInvalidAsync");
            if (method == null) continue;
            
            foreach (var validator in validators)
            {
                if (validator == default)
                {
                    continue;
                }

                var task = (Task?)method.Invoke(validator, new object[] { pair.Value, CancellationToken.None });
                if (task != null)
                {
                    await task;
                }
            }
        }
        
        await next();
    }

    private bool IsReferenceType(Type type)
    {
        return !type.GetTypeInfo().IsValueType && type != typeof(string) && type != typeof(CancellationToken);
    }
}
