using Application;
using RabbitMQ.AMQP.Client;
using System.Text;

namespace WebApi
{
    public static class WebAppExtensions
    {
        public static async Task<WebApplication> StartConsumeRabbitMqMessages(this WebApplication app)
        {
            IServiceScope scope = app.Services.CreateScope();
            IRabbitMq rabbitMq = scope.ServiceProvider.GetRequiredService<IRabbitMq>();

            await rabbitMq.StartConsumeMessages();

            return app;
        }
    }
}