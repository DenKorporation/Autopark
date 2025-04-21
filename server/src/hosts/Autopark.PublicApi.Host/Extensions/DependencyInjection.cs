using Asp.Versioning;
using Autopark.Common.Web.Extensions;
using Autopark.Common.Web.Filters;
using Autopark.PublicApi.Bl.Extensions;
using Autopark.PublicApi.Host.OpenApi;
using Autopark.PublicApi.Models.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Autopark.PublicApi.Host.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        services.AddPublicApiDbContext(configuration);
        services.AddMassTransit(configuration, environment);
        
        services.AddControllers().ConfigurePublicApiMvc(environment);
        services.ConfigureApiVersioning();

        services.AddOptions();
        services.ConfigureOpenIdAuth(configuration);
        services.AddAuthorization();

        services.AddProblemDetails();
        services.AddAutoparkAutoMapper();
        services.ConfigureOpenApi();
        services.ConfigureCors();
        
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        return services;
    }

    private static IServiceCollection ConfigureApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(
            options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            }).AddApiExplorer(
            options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

        return services;
    }

    private static IServiceCollection ConfigureOpenApi(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.ConfigureOptions<ConfigureSwaggerGenOptions>();

        return services;
    }
    
    private static IServiceCollection ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(
            corsOptions =>
            {
                corsOptions.AddPolicy(
                    "DefaultCorsPolicy",
                    policy =>
                    {
                        policy
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

        return services;
    }
    
    private static IMvcBuilder ConfigurePublicApiMvc(this IMvcBuilder mvcBuilder, IHostEnvironment environment)
    {
        mvcBuilder.AddMvcOptions(options =>
        {
            options.Filters.Add(typeof(PermissionFilter));
            options.Filters.Add(typeof(ValidationFilter));
        });

        return mvcBuilder;
    }
}
