namespace Application
{
    public interface IRabbitMq
    {
        Task StartConsumeMessages(CancellationToken cancellationToken = default);
    }
}