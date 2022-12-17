using MimeKit;

namespace AttendanceManager.Application.Models.Mail
{
    public sealed class Message
    {
        public MailboxAddress To { get; private init; }
        public string Subject { get; private init; }
        public string Body { get; private init; }
        public Message(string to, string subject, string body, string name)
        {
            To = new MailboxAddress(name, to);
            Subject = subject;
            Body = body;
        }
    }
}
