using Application;
using Infrastructure;
using RabbitMQ.AMQP.Client;
using RabbitMQ.AMQP.Client.Impl;
using Serilog;
using Shared;
using System.Text.RegularExpressions;

namespace WebApi
{
    public static class WebAppBuilderExtensions
    {
        public static WebApplicationBuilder AddLogger(this WebApplicationBuilder builder)
        {
            builder.Services.AddSerilog((services, lc) => lc.ReadFrom.Configuration(builder.Configuration)
                .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
                .Enrich.WithProperty("Version", WebApiAssembly.GetVersion()));

            return builder;
        }

        public static WebApplicationBuilder AddHealthChecks(this WebApplicationBuilder builder)
        {
            string rabbitMqConnectionString = builder.Configuration.GetRequiredConnectionString(ConnectionStringNames.RabbitMq);
            string rabbitMqAddress = Regex.Match(rabbitMqConnectionString, @"(?<=@)[^\/]+").Value;

            builder.Services.AddHealthChecks()
                            .AddCheck<RabbitMqHealthCheck>($"{ConnectionStringNames.RabbitMq}", tags: [HealthCheckImpactTag.Critical.ToString(), rabbitMqAddress], timeout: TimeSpan.FromSeconds(2));

            return builder;
        }

        public static async Task<WebApplicationBuilder> AddRabbitMqAsync(this WebApplicationBuilder builder)
        {
            string rabbitMqConnectionString = builder.Configuration.GetRequiredConnectionString(ConnectionStringNames.RabbitMq);

            ConnectionSettings settings = ConnectionSettingsBuilder.Create()
                .Uri(new Uri(rabbitMqConnectionString))
                .Build();

            IEnvironment environment = AmqpEnvironment.Create(settings);
            IConnection connection = await environment.CreateConnectionAsync();

            builder.Services.AddSingleton(connection);

            return builder;
        }

        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IRabbitMq, RabbitMqService>();
            builder.Services.AddScoped<INotification, NotificationService>();

            return builder;
        }
    }
}