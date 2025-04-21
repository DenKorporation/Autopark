using Autopark.ApiGateway.Host.Constants;
using Autopark.ApiGateway.Host.Extensions;
using Autopark.Common.Mapping;
using Autopark.Common.Web.Extensions;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAutofac(builder.Configuration);

builder.Services.AddServices(builder.Configuration, builder.Environment);

var app = builder.Build();

var mapper = app.Services.GetRequiredService<IMapper>();
mapper.AssertConfigurationIsValid();

app.UseCors(CorsPolicies.AngularPolicy);

if (app.Environment.IsDevelopment() || app.Environment.IsDocker())
{
    app.UseDeveloperExceptionPage();
    app.UseSwaggerForOcelotUI();
}

app.UseWebSockets();

await app.UseOcelot();

app.Run();
