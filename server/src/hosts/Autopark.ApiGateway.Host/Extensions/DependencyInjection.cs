using System.Text;
using System.Text.RegularExpressions;
using Autopark.ApiGateway.Host.Constants;
using Autopark.Common.Web.Extensions;
using MMLib.SwaggerForOcelot.Configuration;
using Newtonsoft.Json;
using Ocelot.DependencyInjection;

namespace Autopark.ApiGateway.Host.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(
        this IServiceCollection services,
        IConfigurationManager configuration,
        IWebHostEnvironment environment)
    {
        services.AddAutoparkAutoMapper();
        services.ConfigureCors(configuration);
        services.AddOptions();
        services.ConfigureOcelot(configuration, environment);
        services.AddEndpointsApiExplorer();
        services.AddSignalR();

        return services;
    }

    private static IServiceCollection ConfigureOcelot(
        this IServiceCollection services,
        IConfigurationManager configuration,
        IWebHostEnvironment environment)
    {
        configuration.AddOcelotFiles("Routes", environment.EnvironmentName);

        services.AddOcelot(configuration);
        services.AddSwaggerForOcelot(configuration);

        return services;
    }

    private static IServiceCollection ConfigureCors(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddCors(
            corsOptions =>
            {
                corsOptions.AddPolicy(
                    CorsPolicies.AngularPolicy,
                    policy =>
                    {
                        policy
                            .WithOrigins(configuration.GetRequiredSection("Clients:AngularUrl").Value!)
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

        return services;
    }

    private static IConfigurationManager AddOcelotFiles(
        this IConfigurationManager configuration,
        string folder,
        string? nameEnvironment)
    {
        string contents = JsonConvert.SerializeObject(
            MergeFilesOfOcelotConfiguration(
                GetListOfOcelotFiles(
                    folder,
                    nameEnvironment),
                "ocelot.SwaggerEndPoints",
                nameEnvironment));

        configuration.AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(contents)));

        return configuration;
    }

    private static List<FileInfo> GetListOfOcelotFiles(string folder, string? nameEnvironment)
    {
        var reg = new Regex("^ocelot\\.(.*?)\\.json$", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        var source = new DirectoryInfo(folder).EnumerateFiles().Where(fi => reg.IsMatch(fi.Name));
        if (!string.IsNullOrWhiteSpace(nameEnvironment))
            source = source.Where(fi => fi.Name.Contains(nameEnvironment));

        return source.ToList();
    }

    private static SwaggerFileConfiguration MergeFilesOfOcelotConfiguration(
        List<FileInfo> files,
        string fileOfSwaggerEndPoints,
        string? environmentName)
    {
        var fileConfiguration1 = new SwaggerFileConfiguration();

        foreach (var file in files)
        {
            var fileConfiguration2 =
                JsonConvert.DeserializeObject<SwaggerFileConfiguration>(File.ReadAllText(file.FullName));

            if (fileConfiguration2 is null)
            {
                throw new InvalidOperationException($"File {file.Name} doesn't contain ocelot configuration.");
            }

            if (IsGlobalConfigurationFile(environmentName, file.Name, "ocelot.global"))
                fileConfiguration1.GlobalConfiguration = fileConfiguration2.GlobalConfiguration;
            else if (IsGlobalConfigurationFile(environmentName, file.Name, fileOfSwaggerEndPoints))
                fileConfiguration1.SwaggerEndPoints = fileConfiguration2.SwaggerEndPoints;

            fileConfiguration1.Aggregates.AddRange(fileConfiguration2.Aggregates);
            fileConfiguration1.Routes.AddRange(fileConfiguration2.Routes);
        }

        return fileConfiguration1;
    }

    private static bool IsGlobalConfigurationFile(
        string? environmentName,
        string fileName,
        string fileConfigurationName)
    {
        return !string.IsNullOrWhiteSpace(environmentName)
            ? fileName.Equals(
                fileConfigurationName + "." + environmentName + ".json",
                StringComparison.OrdinalIgnoreCase)
            : fileName.Equals(fileConfigurationName + ".json", StringComparison.OrdinalIgnoreCase);
    }
}
