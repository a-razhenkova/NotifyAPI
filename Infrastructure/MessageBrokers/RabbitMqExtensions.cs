using Application;
using RabbitMQ.AMQP.Client;
using Shared;
using System.Text;
using System.Text.Json;

namespace Infrastructure
{
    public static class RabbitMqExtensions
    {
        public static async Task<IConsumer> StartConsumeMessages<TEvent>(this IConnection connection,Func<TEvent, CancellationToken, Task> eventProcessCallback, CancellationToken cancellationToken = default)
        {
            var settings = typeof(TEvent).GetRequiredCustomAttribute<RabbitMqEventAttribute>();

            await connection.EnsureQueueDeclaration(settings);

            IConsumerBuilder consumer = connection.ConsumerBuilder()
                .Queue(settings.QueueName)
                .MessageHandler(async (ctx, message) =>
                {
                    TEvent evt = message.Deserialize<TEvent>();

                    await eventProcessCallback(evt, cancellationToken);

                    ctx.Accept();
                });

            return await consumer.BuildAndStartAsync();
        }

        public static async Task<IQueueSpecification> EnsureQueueDeclaration(this IConnection connection, RabbitMqEventAttribute settings)
        {
            using IManagement management = connection.Management();

            IQueueSpecification queue = management
                .Queue(settings.QueueName)
                .Type(QueueType.CLASSIC)
                .Exclusive(settings.IsExclusive)
                .AutoDelete(settings.AutoDeleteQueue);

            await queue.DeclareAsync();
            return queue;
        }

        private static TEvent Deserialize<TEvent>(this IMessage message)
        {
            string json = Encoding.UTF8.GetString(message.Body());

            return JsonSerializer.Deserialize<TEvent>(json)
                ?? throw new ArgumentNullException("Empty message.");
        }
    }
}