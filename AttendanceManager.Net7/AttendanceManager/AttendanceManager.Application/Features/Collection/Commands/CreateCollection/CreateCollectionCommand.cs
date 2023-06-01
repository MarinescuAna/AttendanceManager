using AttendanceManager.Application.Contracts.Infrastructure.Singleton;
using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Domain.Common;
using AttendanceManager.Domain.Enums;
using MediatR;

namespace AttendanceManager.Application.Features.Collection.Commands.CreateCollection
{
    public sealed class CreateCollectionCommand : IRequest<int>
    {
        public required DateTime ActivityDateTime { get; init; }
        public required CourseType CourseType { get; init; }
        public required string Username { get; init; }
    }

    public sealed class CreateCollectionCommandHandler : IRequestHandler<CreateCollectionCommand, int>
    {
        private IReportSingleton _currentReport;
        private IUnitOfWork _unitOfWork;
        public CreateCollectionCommandHandler(IUnitOfWork unit, IReportSingleton reportSingleton)
        {
            _currentReport = reportSingleton;
            _unitOfWork = unit;

            if (_currentReport.CurrentReportInfo == null)
            {
                throw new Exceptions.NotImplementedException(ErrorMessages.NoContentReportBaseMessage);
            }
        }

        public async Task<int> Handle(CreateCollectionCommand request, CancellationToken cancellationToken)
        {
            var currentReportId = _currentReport.CurrentReportInfo.ReportId;
            var attendanceCollection = new Domain.Entities.AttendanceCollection
            {
                DocumentID = currentReportId,
                HeldOn = request.ActivityDateTime,
                CourseType = request.CourseType,
                Order = _currentReport.LastCollectionOrder[request.CourseType] + 1
            };

            _unitOfWork.AttendanceCollectionRepository.AddAsync(attendanceCollection);

            if (!await _unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrongGenericMessage);
            }

            //update the singleton
            _currentReport.ReportCollectionTypes.Add(attendanceCollection.AttendanceCollectionID, attendanceCollection.CourseType);
            _currentReport.LastCollectionOrder[request.CourseType]++;

            // get all the students according to the document data
            var students = await _unitOfWork.UserSpecializationRepository.GetUserSpecializationsByExpression(
                us => us.User!.EnrollmentYear == _currentReport.CurrentReportInfo.EnrollmentYear &&
                    us.SpecializationID == _currentReport.CurrentReportInfo.SpecializationId &&
                    us.User!.Role == Role.Student &&
                    !us.User.IsDeleted);

            // add each student as not present and with 0 bonus points
            foreach (var student in students)
            {
                _unitOfWork.AttendanceRepository.AddAsync(new Domain.Entities.Attendance
                {
                    UpdatedOn = DateTime.Now,
                    CreatedOn = DateTime.Now,
                    AttendanceCollectionID = attendanceCollection.AttendanceCollectionID,
                    BonusPoints = 0,
                    IsPresent = false,
                    UserID = student.UserID,
                });
            }

            if (!await _unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrongGenericMessage);
            }

            //send notifications to each user 
            foreach (var user in students)
            {
                _unitOfWork.NotificationRepository.AddAsync(new()
                {
                    Priority = Domain.Enums.NotificationPriority.Info,
                    UserID = user.UserID,
                    CreatedOn = DateTime.Now,
                    IsRead = false,
                    Message = string.Format(NotificationMessages.CreateCollectionNotification, request.Username, _currentReport.CurrentReportInfo.Title)
                });
            }

            // Save the members 
            if (!await _unitOfWork.CommitAsync(students.Count()))
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrong_CreateReportNotificationInsert_Error);
            }

            return attendanceCollection.AttendanceCollectionID;
        }
    }
}
