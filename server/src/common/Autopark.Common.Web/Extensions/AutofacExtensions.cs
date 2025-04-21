using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autopark.Common.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Autopark.Common.Web.Extensions;

public static class AutofacExtensions
{
    public static void ConfigureAutofac(this IHostBuilder hostBuilder, IConfiguration configuration)
    {
        ReflectionHelper.LoadAssembliesFromApplicationDirectory("Autopark.*.dll");
        hostBuilder.UseServiceProviderFactory(new AutofacServiceProviderFactory());

        hostBuilder.ConfigureContainer<ContainerBuilder>(
            builder =>
            {
                // builder.RegisterInstance(configuration);
                builder.RegisterAssemblyModules(AppDomain.CurrentDomain.GetAssemblies());
            });
    }
}
