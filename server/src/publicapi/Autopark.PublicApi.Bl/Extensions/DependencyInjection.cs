using Autopark.Common.Web.Extensions;
using Confluent.Kafka;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Autopark.PublicApi.Bl.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddMassTransit(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        if (environment.IsTesting())
        {
            return services;
        }

        services.AddMassTransit(
            x =>
            {
                var kafkaHost = configuration.GetValue<string>("Kafka:Host");

                x.UsingInMemory();

                x.AddRider(
                    rider =>
                    {
                        rider.UsingKafka(
                            (_, configurator) =>
                            {
                                configurator.Acks = Acks.Leader;
                                configurator.Host(kafkaHost);
                            });
                    });
            });

        return services;
    }
}
