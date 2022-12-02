using AttendanceManager.Application.Models.Mail;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AttendanceManager.Infrastructure.Mail
{
    public class MailService : Application.Contracts.Mail.IMailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> options)
        {
            _mailSettings = options.Value;
        }

        public async Task<bool> SendEmail(Message message, CancellationToken ct = default)
        {
            var mailMessage = CreateMessage(message);

            using (var smtp = new SmtpClient())
            {
                try
                {
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
                catch (Exception)
                {
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
