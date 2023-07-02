using AttendanceManagerGenerator.Modules;
using AttendanceManagerGenerator.Utils;

namespace AttendanceManagerGenerator.Handlers
{
    public sealed class ReportHandler
    {
        private readonly RequestHandler _handler;
        private readonly CollectionHandler _collectionHandler;
        private readonly InvolvementsHandler _involvementsHandler;
        public ReportHandler(HttpClient http)
        {
            _handler = new RequestHandler(http, "report");
            _collectionHandler = new CollectionHandler(http, RandomGenerator.Next(0,2)==1);
            _involvementsHandler = new InvolvementsHandler(http);
        }

        public async Task InsertReportAsync(Report report)
        {
            var newReport = await AddReportAsync(report) ?? throw new Exception("Process faild!");

            await Console.Out.WriteLineAsync("Report added!");

            var currentReport = await _handler.GetAsync<ReportGetResponseVm>($"{newReport.DocumentId}") ?? throw new Exception("Not found!");

            await InsertCollectionsAsync(ActivityType.Lecture, currentReport.MaxNoLessons);
            await Console.Out.WriteLineAsync("All lectures was added!");

            if (currentReport.MaxNoLaboratories != 0)
            {
                await InsertCollectionsAsync(ActivityType.Laboratory, currentReport.MaxNoLaboratories);
                await Console.Out.WriteLineAsync("All laboratories was added!");
            }

            if (currentReport.MaxNoSeminaries != 0)
            {
                await InsertCollectionsAsync(ActivityType.Seminary, currentReport.MaxNoSeminaries);
                await Console.Out.WriteLineAsync("All seminaries was added!");
            }
        }

        private async Task<ReportPostResponseVm?> AddReportAsync(Report report)
        {
            var attendanceWeight = RandomGenerator.Next(0, 100);
            var newReport = new ReportPostVm()
            {
                Title = string.IsNullOrEmpty(report.Title)? $"Generated_{Guid.NewGuid()}":report.Title,
                AttendanceImportance = attendanceWeight,
                BonusPointsImportance = 100 - attendanceWeight,
                MaxNoLessons = RandomGenerator.Next(5, 12),
                MaxNoLaboratories = RandomGenerator.Next(0, 12),
                MaxNoSeminaries = RandomGenerator.Next(0, 6),
                CourseId = report.CourseId,
                EnrollmentYear = report.EnrollmentYear,
                SpecializationId = report.SpecializationId
            };

            var users = await _handler.GetAsync<List<StudentGetVm>>($"students?year={report.EnrollmentYear}&specializationId={report.SpecializationId}", "user");

            if (users != null)
            {
                newReport.StudentIds = users.Select(s => s.Email).ToArray();
            }

            return await _handler.PostAsync<ReportPostVm, ReportPostResponseVm>("create_report", newReport);
        }

        private async Task InsertCollectionsAsync(ActivityType activityType, int entities)
        {
            for (var i = 0; i < entities; i++)
            {
                var result = await _collectionHandler.InsertCollectionAsync(activityType);
                await Console.Out.WriteLineAsync($"Collection {result} was added!");

                if (result != null && result!=0)
                {
                    await _involvementsHandler.InsertInvolvementsAsync((int)result);
                }
            }
        }
    }
}
