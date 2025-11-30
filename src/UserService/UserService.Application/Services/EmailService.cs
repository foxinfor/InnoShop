using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using UserService.Application.Interfaces;

namespace UserService.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendConfirmationEmailAsync(string toEmail, string token, CancellationToken cancellationToken = default)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("InnoShop", _configuration["EmailSettings:From"]));
            message.To.Add(new MailboxAddress(toEmail, toEmail));
            message.Subject = "Подтверждение аккаунта";

            message.Body = new TextPart(TextFormat.Plain)
            {
                Text = $"Ваш токен подтверждения: {token}\n" +
                       $"Введите его в приложении для завершения регистрации."
            };

            using var client = new MailKit.Net.Smtp.SmtpClient();

            await client.ConnectAsync(_configuration["EmailSettings:SmtpServer"],
                                      int.Parse(_configuration["EmailSettings:Port"]),
                                      SecureSocketOptions.StartTls, cancellationToken);

            await client.AuthenticateAsync(_configuration["EmailSettings:Username"],
                                           _configuration["EmailSettings:Password"], cancellationToken);

            await client.SendAsync(message, cancellationToken);
            await client.DisconnectAsync(true, cancellationToken);
        }


        public async Task SendPasswordResetEmailAsync(string toEmail, string token, CancellationToken cancellationToken = default)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("InnoShop", _configuration["EmailSettings:From"]));
            message.To.Add(new MailboxAddress(toEmail, toEmail));
            message.Subject = "Восстановление пароля";

            message.Body = new TextPart(TextFormat.Plain)
            {
                Text = $"Вы запросили восстановление пароля.\n" +
                       $"Ваш токен: {token}\n" +
                       $"Введите его в приложении вместе с новым паролем."
            };

            using var client = new SmtpClient();
            await client.ConnectAsync(_configuration["EmailSettings:SmtpServer"],
                                      int.Parse(_configuration["EmailSettings:Port"]),
                                      SecureSocketOptions.StartTls, cancellationToken);

            await client.AuthenticateAsync(_configuration["EmailSettings:Username"],
                                           _configuration["EmailSettings:Password"], cancellationToken);

            await client.SendAsync(message, cancellationToken);
            await client.DisconnectAsync(true, cancellationToken);
        }

    }
}
