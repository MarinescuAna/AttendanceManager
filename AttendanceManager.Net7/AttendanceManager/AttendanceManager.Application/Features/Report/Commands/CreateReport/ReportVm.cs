namespace AttendanceManager.Application.Features.Report.Commands.CreateReport
{
    public sealed class ReportVm
    {
        public required int DocumentId { get; set; }
        public required string UpdatedOn { get; set; }
    }
}
