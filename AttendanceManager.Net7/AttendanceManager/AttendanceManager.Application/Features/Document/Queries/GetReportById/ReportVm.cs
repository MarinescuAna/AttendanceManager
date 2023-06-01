namespace AttendanceManager.Application.Features.Document.Queries.GetReportById
{
    public sealed class ReportVm
    {
        public required int ReportId { get; init; }
        public required string Title { get; init; }
        public required int EnrollmentYear { get; init; }
        public required string CourseName { get; init; }
        public required string SpecializationName { get; set; }
        public required string UpdatedOn { get; set; }
        public required int MaxNoSeminaries { get; init; }
        public required int MaxNoLaboratories { get; init; }
        public required int MaxNoLessons { get; init; }
        public required int NoSeminaries { get; init; }
        public required int NoLaboratories { get; init; }
        public required int NoLessons { get; init; }
        public required int CourseId { get; init; }
        public required int SpecializationId { get; set; }
        public required string CreationDate { get; init; }
        public required int AttendanceImportance { get; init; }
        public required int BonusPointsImportance { get; init; }
        public required string CreatedBy { get; init; }
        public required bool IsCreator { get; init; }
        public required int NumberOfStudents { get; init; }
        public required CollectionDto[] Collections { get; init; }
        public required MembersDto[] Members { get; init; }
    }

    public class CollectionDto
    {
        public required int CollectionId { get; init; }
        public required string ActivityTime { get; init; }
        public required string CourseType { get; init; }
    }
    public sealed class MembersDto
    {
        public required string Email { get; init; }
        public required string Name { get; init; }
    }
}