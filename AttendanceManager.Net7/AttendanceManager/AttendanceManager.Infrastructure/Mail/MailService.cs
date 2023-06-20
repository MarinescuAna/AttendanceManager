using AttendanceManager.Application.Contracts.Infrastructure.Logging;
using AttendanceManager.Application.Contracts.Infrastructure.Mail;
using AttendanceManager.Application.Models.Mail;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace AttendanceManager.Infrastructure.Mail
{
    public sealed class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        private readonly ILoggingService _loggingService;
        public MailService(IOptions<MailSettings> options, ILoggingService loggingService)
        {
            _loggingService = loggingService;
            _mailSettings = options.Value;
        }

        public async Task<bool> SendEmailAsync(Message message, CancellationToken ct = default)
        {
            // Create the message that will be sent
            var mailMessage = CreateMessage(message);

            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    // Send email
                    if (_mailSettings.UseSSL)
                    {
                        await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.SslOnConnect, ct);
                    }
                    else if (_mailSettings.UseStartTls)
                    {
                        await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls, ct);
                    }

                    await smtp.AuthenticateAsync(_mailSettings.User, _mailSettings.Password, ct);
                    await smtp.SendAsync(mailMessage, ct);

                    return true;
                }
                catch (Exception ex)
                {
                    _loggingService.LogException(ex, System.Reflection.MethodBase.GetCurrentMethod()?.Name);
                    return false;
                }
                finally
                {
                    await smtp.DisconnectAsync(true, ct);
                    smtp.Dispose();
                }
            }
        }

        private MimeMessage CreateMessage(Message message)
        {
            var email = new MimeMessage
            {
                Subject = message.Subject,
                Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = message.Body
                }

            };
            email.From.Add(new MailboxAddress(_mailSettings.From, _mailSettings.User));
            email.To.Add(message.To);

            return email;
        }


    }
}
