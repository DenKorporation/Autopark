using Asp.Versioning.ApiExplorer;
using Autopark.Common.Configuration;
using Autopark.Common.Mapping;
using Autopark.Common.Web.Extensions;
using Autopark.PublicApi.Bl.Extensions;
using Autopark.PublicApi.Host.Extensions;
using Autopark.PublicApi.Models.Extensions;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAutofac(builder.Configuration);

builder.Services.AddServices(builder.Configuration, builder.Environment);

var app = builder.Build();

var mapper = app.Services.GetRequiredService<IMapper>();
mapper.AssertConfigurationIsValid();

app.UseCors("DefaultCorsPolicy");

app.UseErrorHandler();

if (app.Environment.IsDevelopment() || app.Environment.IsDocker())
{
    var config = app.Services.GetRequiredService<IOptions<IdentityConfiguration>>();

    app.UseSwagger();
    app.UseSwaggerUI(
        options =>
        {
            IReadOnlyList<ApiVersionDescription> descriptions = app.DescribeApiVersions();

            foreach (var description in descriptions)
            {
                string url = $"/swagger/{description.GroupName}/swagger.json";
                string name = description.GroupName.ToUpperInvariant();

                options.SwaggerEndpoint(url, name);
            }

            options.OAuthClientId(config.Value.ClientId);
        });

    var groupIds = await app.InitializeKeycloakUsersAsync();
    await app.InitializeDatabaseAsync(groupIds);
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
}
