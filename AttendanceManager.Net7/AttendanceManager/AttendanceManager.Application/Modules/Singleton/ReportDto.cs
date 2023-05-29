
using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;

namespace AttendanceManager.Application.Modules.Singleton
{
    public sealed class ReportDto
    {
        public int ReportId { get; init; }
        public int EnrollmentYear { get; init; }
        public int SpecializationId { get; init; }
        // UPDATE this when update the report
        public int MaxNumberOfLectures { get; }
        // UPDATE this when update the report
        public int MaxNumberOfLaboratories { get; }
        // UPDATE this when update the report
        public int MaxNumberOfSeminaries { get; }

        public ReportDto(Document report)
        {
            ReportId = report.DocumentId;
            EnrollmentYear = report.EnrollmentYear;
            SpecializationId = report.Course!.UserSpecialization!.SpecializationID;

            MaxNumberOfLectures = report.MaxNoLessons;
            MaxNumberOfLaboratories= report.MaxNoLaboratories;
            MaxNumberOfSeminaries = report.MaxNoSeminaries;
        }

    }
}
