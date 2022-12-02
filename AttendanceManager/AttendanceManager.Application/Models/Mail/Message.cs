using MimeKit;

namespace AttendanceManager.Application.Models.Mail
{
    public class Message
    {
        public MailboxAddress To { get; }
        public string Subject { get; }
        public string Body { get; }
        public Message(string to, string subject, string body, string name)
        {
            To = new MailboxAddress(name, to);
            Subject = subject;
            Body = body;
        }
    }
}
