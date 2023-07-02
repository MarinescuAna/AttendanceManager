namespace AttendanceManagerGenerator.Modules
{
    public sealed class ReportPostVm
    {
        public  required string Title { get; set; }
        public int EnrollmentYear { get; set; }
        public int MaxNoSeminaries { get; set; }
        public int MaxNoLaboratories { get; set; }
        public int MaxNoLessons { get; set; }
        public int CourseId { get; set; }
        public int SpecializationId { get; set; }
        public int AttendanceImportance { get; set; }
        public int BonusPointsImportance { get; set; }
        public string[] StudentIds { get; set; }
    }
}
