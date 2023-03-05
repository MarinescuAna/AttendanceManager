﻿namespace AttendanceManager.Application.Features.Attendance.Queries.GetStudentAttendanceByDocIdAndUserId
{
    public sealed class StudentAttendancesDTO
    {
        public required int BonusPoints { get; init; }
        public required bool WasPresent { get; init; }
        public required string CourseType { get; init; }
        public required string UpdatedOn { get; init; }
        public required int AttendanceId { get; init; }
    }
}
