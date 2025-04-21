using System.Reflection;
using Autofac;

namespace Autopark.Common.Extensions;

public static class OpenGenericRegistrationExtensions
{
    public static OpenGenericRegistrationBuilder RegisterAssemblyOpenGenericTypes(
        this ContainerBuilder builder,
        params Assembly[] assemblies)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        return new OpenGenericRegistrationBuilder(builder, assemblies);
    }
}
