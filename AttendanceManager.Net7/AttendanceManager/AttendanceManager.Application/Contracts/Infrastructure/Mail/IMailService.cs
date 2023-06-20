using AttendanceManager.Application.Models.Mail;

namespace AttendanceManager.Application.Contracts.Infrastructure.Mail
{
    public interface IMailService
    {
        Task<bool> SendEmailAsync(Message message, CancellationToken ct = default);
    }
}
