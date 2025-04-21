using Autopark.Common.Extensions;

namespace Autopark.Common.Web.Security;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class PermissionAttribute : Attribute
{
    public PermissionAttribute(string roles, params Type[] ruleTypes) : this(ruleTypes)
    {
        var roleList = roles.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        var allRoles = RoleExtensions.GetAllRoles();
        foreach (var role in roleList)
        {
            if (!allRoles.Contains(role))
            {
                throw new ArgumentException($"There is no such role as {role}");
            }
        }

        Roles = roles;
    }

    public PermissionAttribute(params Type[] ruleTypes)
    {
        foreach (var ruleType in ruleTypes)
        {
            if (!typeof(IPermissionRule).IsAssignableFrom(ruleType))
            {
                throw new ArgumentException($"{ruleType.FullName} must inherit {typeof(IPermissionRule).FullName}");
            }
        }

        RuleTypes = ruleTypes;
    }

    public Type[] RuleTypes { get; }

    public string? Roles { get; init; }
}