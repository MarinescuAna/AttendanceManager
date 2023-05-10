using AttendanceManager.Application.Models.Mail;

namespace AttendanceManager.Application.Contracts.Infrastructure.Mail
{
    public interface IMailService
    {
        Task<bool> SendEmail(Message message, CancellationToken ct = default);
    }
}
