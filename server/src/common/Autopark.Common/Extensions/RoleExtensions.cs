using System.Reflection;
using Autopark.Common.Constants;

namespace Autopark.Common.Extensions;

public static class RoleExtensions
{
    public static IList<string> GetAllRoles()
    {
        var type = typeof(Roles);
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

        return fields
            .Where(x => x.IsLiteral && !x.IsInitOnly && x.FieldType == typeof(string))
            .Select(x => (string)x.GetValue(null)!)
            .ToList();
    }
}
