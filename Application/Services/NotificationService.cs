namespace Application
{
    public class NotificationService : INotification
    {
        public NotificationService()
        {

        }

        public async Task SendEmail(string email, string message, CancellationToken cancellationToken = default)
        {
            // TODO
            throw new NotImplementedException();
        }
    }
}