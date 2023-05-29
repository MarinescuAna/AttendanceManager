using AttendanceManager.Application.Contracts.Infrastructure.Singleton;
using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Domain.Common;
using AttendanceManager.Domain.Enums;
using MediatR;

namespace AttendanceManager.Application.Features.AttendanceCollection.Commands.CreateAttendanceCollection
{
    public sealed class CreateAttendanceCollectionCommand : IRequest<int>
    {
        public required string ActivityDateTime { get; init; }
        public required string CourseType { get; init; }
    }

    public sealed class CreateAttendanceCollectionCommandHandler : IRequestHandler<CreateAttendanceCollectionCommand, int>
    {
        private IReportSingleton _currentReport;
        private IUnitOfWork _unitOfWork;
        public CreateAttendanceCollectionCommandHandler(IUnitOfWork unit, IReportSingleton reportSingleton)
        {
            _currentReport = reportSingleton;
            _unitOfWork = unit;

            if (_currentReport.CurrentReportInfo == null)
            {
                throw new NotImplementedException(ErrorMessages.NoContentReportBaseMessage);
            }
        }

        public async Task<int> Handle(CreateAttendanceCollectionCommand request, CancellationToken cancellationToken)
        {
            var currentReportId = _currentReport.CurrentReportInfo.ReportId;

            var courseType = (CourseType)Enum.Parse(typeof(CourseType), request.CourseType);
            var attendanceCollection = new Domain.Entities.AttendanceCollection
            {
                DocumentID = currentReportId,
                HeldOn = DateTime.Parse(request.ActivityDateTime),
                CourseType = courseType,
                Order = _currentReport.LastCollectionOrder[courseType] + 1
            };

            _unitOfWork.AttendanceCollectionRepository.AddAsync(attendanceCollection);

            if (!await _unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrongGenericMessage);
            }

            //update the singleton
            _currentReport.ReportCollectionTypes.Add(attendanceCollection.AttendanceCollectionID, attendanceCollection.CourseType);
            _currentReport.LastCollectionOrder[courseType]++;

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

            return attendanceCollection.AttendanceCollectionID;
        }
    }
}
