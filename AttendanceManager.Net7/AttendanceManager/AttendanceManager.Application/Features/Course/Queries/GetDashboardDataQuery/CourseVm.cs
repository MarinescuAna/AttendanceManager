using AttendanceManager.Domain.Enums;

namespace AttendanceManager.Application.Features.Course.Queries.GetDashboardDataQuery
{
    public class CourseVm
    {
        public required int CourseId { get; init; } 
        public required string CourseName { get; init; }
        public ReportDto[] Reports { get; init; }
    }

    public class ReportDto
    {
        public required int ReportId { get; init; }
        public required string ReportName { get; init; }
        public required float[] PercentageAttendances { get; init; } = new float[4];
        public required int[] NoAttendancesAchieved { get; init; } = new int[4];
        public required int[] NoPointsAchieved { get; init; } = new int[4];
        public required int CreationYear { get; init; }
        public BadgesDto[] Badges { get; set; }
    }

    public class BadgesDto
    {
        public required string Title { get; init; }
        public required string Description { get; init; }
        public required string ImagePath { get; init; }
        public required int NoAchievements { get; init; }
        public required int BadgeType { get; init; }
        public required float PercentageAchievements { get; init; }
        public required Role Role { get; init; }
        public required int MaxAchievements { get; init; }
    }
}
