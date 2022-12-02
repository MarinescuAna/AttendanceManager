using AttendanceManager.Application.Models.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace AttendanceManager.Application.Contracts.Mail
{
    public interface IMailService
    {
        Task<bool> SendEmail(Message message, CancellationToken ct = default);
    }
}
