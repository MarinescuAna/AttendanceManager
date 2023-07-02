using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Domain.Common;
using MediatR;

namespace AttendanceManager.Application.Features.Report.Commands.CreateReport
{
    public sealed class CreateReportCommand : IRequest<ReportVm>
    {
        public required string Title { get; init; }
        public required int EnrollmentYear { get; init; }
        public string? Email { get; set; }
        public required int MaxNoSeminaries { get; init; }
        public required int MaxNoLaboratories { get; init; }
        public required int MaxNoLessons { get; init; }
        public required int CourseId { get; init; }
        public required int SpecializationId { get; set; }
        public required int AttendanceImportance { get; init; }
        public required int BonusPointsImportance { get; init; }
        public required string[] StudentIds { get; set; }

    }

    public sealed class CreateReportCommandHandler : IRequestHandler<CreateReportCommand, ReportVm>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateReportCommandHandler(IUnitOfWork unit)
        {
            _unitOfWork = unit;
        }

        public async Task<ReportVm> Handle(CreateReportCommand request, CancellationToken cancellationToken)
        {
            var newReport = new Domain.Entities.Report
            {
                CourseID = request.CourseId,
                CreatedOn = DateTime.Now,
                EnrollmentYear = request.EnrollmentYear,
                MaxNoLaboratories = request.MaxNoLaboratories,
                MaxNoLessons = request.MaxNoLessons,
                MaxNoSeminaries = request.MaxNoSeminaries,
                Title = request.Title,
                UpdatedOn = DateTime.Now,
                AttendanceImportance = request.AttendanceImportance,
                BonusPointsImportance = request.BonusPointsImportance
            };
            // Save report first to can get the id
            await _unitOfWork.ReportRepository.AddAsync(newReport);
            if (!await _unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrongGenericMessage);
            }

            // REMEMBER: the teacher that create the report can be access form course, so there is no reason to store it in the Member table

            // insert all the sudents into the report
            await _unitOfWork.MemberRepository.AddRangeAsync(request.StudentIds.Select(s => new Domain.Entities.Member
            {
                MemberID = Guid.NewGuid(),
                ReportID = newReport.ReportID,
                UserID = s,
            }).ToList());


            // Save the members 
            if (!await _unitOfWork.CommitAsync(request.StudentIds.Length))
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrongGenericMessage);
            }

            //send notifications to each user 
            await _unitOfWork.NotificationRepository.AddRangeAsync(request.StudentIds.Select(s => new Domain.Entities.Notification()
            {
                Priority = Domain.Enums.NotificationPriority.Info,
                UserID = s,
                CreatedOn = DateTime.Now,
                IsRead = false,
                Message = string.Format(NotificationMessages.CreateReportNotification, newReport.Title, request.Email, newReport.CreatedOn.ToString(Constants.ShortDateFormat)),
            }));

            // Save the members 
            if (!await _unitOfWork.CommitAsync(request.StudentIds.Length))
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrong_CreateReportNotificationInsert_Error);
            }

            return new()
            {
                UpdatedOn = newReport.UpdatedOn.ToString(Constants.ShortDateFormat),
                DocumentId = newReport.ReportID
            };
        }
    }
}
