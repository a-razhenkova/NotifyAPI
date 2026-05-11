namespace Application
{
    public interface INotification
    {
        Task SendEmail(string email, string message, CancellationToken cancellationToken = default);
    }
}