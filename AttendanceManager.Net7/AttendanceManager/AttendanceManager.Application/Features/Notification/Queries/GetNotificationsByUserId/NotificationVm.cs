namespace AttendanceManager.Application.Features.Notification.Queries.GetNotificationsByUserId
{
    public sealed class NotificationVm
    {
        public required int NotificationId { get; set; }
        public required string Message { get; set; }
        public string? Image { get; set; }
        public required string CreatedOn { get; set; }
        public required Domain.Enums.NotificationPriority Priority { get; set; }
        public required bool IsRead { get; set; }
    }
}
