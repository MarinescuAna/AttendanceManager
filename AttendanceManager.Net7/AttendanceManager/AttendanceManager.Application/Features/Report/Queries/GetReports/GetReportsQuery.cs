using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Domain.Common;
using AttendanceManager.Domain.Enums;
using MediatR;

namespace AttendanceManager.Application.Features.Report.Queries.GetReports
{
    public sealed class GetReportsQuery : IRequest<IEnumerable<ReportVm>>
    {
        public required string Email { get; init; }
        public required Role Role { get; init; }
    }

    public sealed class GetReportsQueryHandler : IRequestHandler<GetReportsQuery, IEnumerable<ReportVm>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetReportsQueryHandler(IUnitOfWork unit)
        {
            _unitOfWork = unit;
        }
        /*
         * Get reports according to the user email and role:
         * - Admin: get all the reports where the user has the role Creator or collaborator
         * - Student: get all the reports where the student has the role Member
         */
        public async Task<IEnumerable<ReportVm>> Handle(GetReportsQuery request, CancellationToken cancellationToken)
        {
            var allReports = new List<ReportVm>();
            var reports = request.Role == Role.Student ?
                await _unitOfWork.ReportRepository.GetUserReportsByExpressionAsync(u =>
                    u.Members!.Any(m => m.UserID == request.Email)) :
                await _unitOfWork.ReportRepository.GetUserReportsByExpressionAsync(u =>
                    (u.Course!.UserSpecialization!.UserID == request.Email || u.Members!.Any(m => m.UserID == request.Email)));

            foreach (var report in reports.OrderByDescending(d => d.CreatedOn))
            {
                allReports.Add(new()
                {
                    IsCreator = request.Role == Role.Student ? false : report.Course!.UserSpecialization!.UserID.Equals(request.Email),
                    CourseName = report.Course!.Name,
                    DocumentId = report.ReportID,
                    EnrollmentYear = report.EnrollmentYear,
                    SpecializationName = report.Course!.UserSpecialization!.Specialization!.Name,
                    Title = report.Title,
                    UpdatedOn = report.UpdatedOn.ToString(Constants.ShortDateFormat)
                });
            }

            return allReports;
        }

    }
}
