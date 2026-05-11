using Application;
using Application.RabbitMq;
using RabbitMQ.AMQP.Client;

namespace Infrastructure
{
    public class RabbitMqService : IRabbitMq
    {
        private readonly IConnection _connection;
        private readonly INotification _notification;

        public RabbitMqService(IConnection connection,
                              INotification notification)
        {
            _connection = connection;
            _notification = notification;
        }

        public async Task StartConsumeMessages(CancellationToken cancellationToken = default)
        {
            await _connection.StartConsumeMessages<EmailVerificationEvent>(SendEmailVerificationEvent);
            await _connection.StartConsumeMessages<NewUserOtpEvent>(SendNewUserOtpEvent);
            await _connection.StartConsumeMessages<LoginFromNewIpAddressEvent>(SendLoginFromNewIpAddressEvent);
            await _connection.StartConsumeMessages<UserPasswordChangedEvent>(SendUserPasswordChangedEvent);
            await _connection.StartConsumeMessages<LoginAttemptMadeEvent>(SendLoginAttemptMadeEvent);
        }

        private async Task SendEmailVerificationEvent(EmailVerificationEvent evt, CancellationToken cancellationToken = default)
            => await _notification.SendEmail(evt.UserEmail, MessageMapper.Map(evt), cancellationToken);

        private async Task SendNewUserOtpEvent(NewUserOtpEvent evt, CancellationToken cancellationToken = default)
            => await _notification.SendEmail(evt.UserEmail, MessageMapper.Map(evt), cancellationToken);

        private async Task SendUserPasswordChangedEvent(UserPasswordChangedEvent evt, CancellationToken cancellationToken = default)
            => await _notification.SendEmail(evt.UserEmail, MessageMapper.Map(evt), cancellationToken);

        private async Task SendLoginFromNewIpAddressEvent(LoginFromNewIpAddressEvent evt, CancellationToken cancellationToken = default)
            => await _notification.SendEmail(evt.UserEmail, MessageMapper.Map(evt), cancellationToken);

        private async Task SendLoginAttemptMadeEvent(LoginAttemptMadeEvent evt, CancellationToken cancellationToken = default)
            => await _notification.SendEmail(evt.UserEmail, MessageMapper.Map(evt), cancellationToken);
    }
}