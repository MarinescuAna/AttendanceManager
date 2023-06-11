
using AttendanceManager.Domain.Entities;

namespace AttendanceManager.Application.Modules.Singleton
{
    public sealed class ReportDto
    {
        public int ReportId { get; init; }
        public int EnrollmentYear { get; init; }
        public int SpecializationId { get; init; }
        // UPDATE this when update the report
        public int MaxNumberOfLectures { get; set; }
        // UPDATE this when update the report
        public int MaxNumberOfLaboratories { get; set; }
        // UPDATE this when update the report
        public int MaxNumberOfSeminaries { get; set; }
        public string Title { get; set; }
        public string CreatedBy { get; init; }
        public int NoOfStudents { get; init; }

        public ReportDto(Document report, int noOfStudents)
        {
            ReportId = report.DocumentId;
            EnrollmentYear = report.EnrollmentYear;
            SpecializationId = report.Course!.UserSpecialization!.SpecializationID;
            Title= report.Title;
            CreatedBy = report.Course.UserSpecialization!.UserID;
            NoOfStudents = noOfStudents;

            MaxNumberOfLectures = report.MaxNoLessons;
            MaxNumberOfLaboratories = report.MaxNoLaboratories;
            MaxNumberOfSeminaries = report.MaxNoSeminaries;
        }

    }
}
