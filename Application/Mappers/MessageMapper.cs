using Application.RabbitMq;
using System.Text;

namespace Application
{
    public static class MessageMapper
    {
        public static string Map(EmailVerificationEvent evt)
        {
            var message = new StringBuilder();
            message.AppendLine($"Dear {evt.Username},");
            message.AppendLine();
            message.AppendLine($"You can verify your token from:");
            message.AppendLine(evt.VerificationToken); // TODO: make it URL
            return message.ToString();
        }

        public static string Map(NewUserOtpEvent evt)
        {
            var message = new StringBuilder();
            message.AppendLine($"Dear {evt.Username},");
            message.AppendLine();
            message.AppendLine($"You OTP is:");
            message.AppendLine(evt.Otp);
            return message.ToString();
        }

        public static string Map(LoginFromNewIpAddressEvent evt)
        {
            var message = new StringBuilder();
            message.AppendLine($"Dear {evt.Username},");
            message.AppendLine();
            message.Append($"A new login was made at {evt.Timestamp}");

            if (string.IsNullOrWhiteSpace(evt.IpAddress))
                message.AppendLine(".");
            else
                message.AppendLine($" from IP address: {evt.IpAddress}");

            return message.ToString();
        }

        public static string Map(UserPasswordChangedEvent evt)
        {
            var message = new StringBuilder();
            message.AppendLine($"Dear {evt.Username},");
            message.AppendLine();
            message.Append($"Your password was changed at {evt.Timestamp}");

            if (string.IsNullOrWhiteSpace(evt.UserIpAddress))
                message.AppendLine(".");
            else
                message.AppendLine($" from IP address: {evt.UserIpAddress}");

            return message.ToString();
        }

        public static string Map(LoginAttemptMadeEvent evt)
        {
            var message = new StringBuilder();
            message.AppendLine($"Dear {evt.Username},");
            message.AppendLine();
            message.Append($"A login attempt was made at {evt.Timestamp}");

            if (string.IsNullOrWhiteSpace(evt.IpAddress))
                message.AppendLine(".");
            else
                message.AppendLine($" from IP address: {evt.IpAddress}");

            return message.ToString();
        }
    }
}