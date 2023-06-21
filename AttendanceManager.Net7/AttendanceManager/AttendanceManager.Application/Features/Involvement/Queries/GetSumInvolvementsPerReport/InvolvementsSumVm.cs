namespace AttendanceManager.Application.Features.Involvement.Queries.GetSumInvolvementsPerReport
{
    public sealed class InvolvementsSumVm
    {
        public required string UserId { get; init; }
        public required string UserName { get; init; }
        public required string Code { get; init; }
        public required int BonusPoints { get; init; }
        public required int CourseAttendances { get; init; }
        public required int LaboratoryAttendances { get; init; }
        public required int SeminaryAttendances { get; init; }

    }
}
