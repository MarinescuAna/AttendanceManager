using AttendanceManager.Application.Contracts.Infrastructure.Singleton;
using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Domain.Common;
using AttendanceManager.Domain.Enums;
using MediatR;
using System.Globalization;

namespace AttendanceManager.Application.Features.Collection.Commands.CreateCollection
{
    public sealed class CreateCollectionCommand : IRequest<int>
    {
        public required string ActivityDateTime { get; init; }
        public required string CourseType { get; init; }
        public string? Username { get; set; }
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

            if (!DateTime.TryParseExact(request.ActivityDateTime, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedActivityTime))
            {
                throw new BadRequestException(ErrorMessages.BadRequest_CreateCollectionParams2_Error);
            }

            if (!Enum.TryParse(request.CourseType, out CourseType courseType))
            {
                throw new BadRequestException(ErrorMessages.BadRequest_CreateCollectionParams3_Error);
            }

            var attendanceCollection = new Domain.Entities.AttendanceCollection
            {
                DocumentID = currentReportId,
                HeldOn = parsedActivityTime,
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
            var students = _currentReport.Members.Where(s => s.Value.Equals(Role.Student));

            // add each student as not present and with 0 bonus points
            foreach (var student in students)
            {
                _unitOfWork.AttendanceRepository.AddAsync(new Domain.Entities.Attendance
                {
                    UpdatedOn = DateTime.Now,
                    AttendanceCollectionID = attendanceCollection.AttendanceCollectionID,
                    BonusPoints = 0,
                    IsPresent = false,
                    UserID = student.Key,
                });
            }

            if (!await _unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrongGenericMessage);
            }

            //send notifications to each user 
            var attendances = attendanceCollection.Order == GetMaxNumberByCourseType(attendanceCollection.CourseType) / 2 ||
                  attendanceCollection.Order == GetMaxNumberByCourseType(attendanceCollection.CourseType) ?
                  _unitOfWork.AttendanceRepository.ListAll().Where(a => a.AttendanceCollection!.DocumentID == _currentReport.CurrentReportInfo.ReportId)
                        .Where(a => a.IsPresent) : null;

            foreach (var user in students)
            {
                //is the half of the activity
                if (attendances != null && attendanceCollection.Order == GetMaxNumberByCourseType(attendanceCollection.CourseType) / 2)
                {
                    var noAttendancesStudent = attendances.Count(a => a.UserID.Equals(user.Key) && a.AttendanceCollection!.CourseType == attendanceCollection.CourseType);
                    //user don't have enough attendances
                    if (noAttendancesStudent < attendanceCollection.Order)
                    {
                        _unitOfWork.NotificationRepository.AddAsync(new()
                        {
                            Priority = Domain.Enums.NotificationPriority.Alert,
                            UserID = user.Key,
                            CreatedOn = DateTime.Now,
                            IsRead = false,
                            Message = string.Format(NotificationMessages.HalfSemesterNoAttendancesNotification,
                                _currentReport.CurrentReportInfo.Title, attendanceCollection.CourseType.ToString(),
                                GetMaxNumberByCourseType(attendanceCollection.CourseType) - noAttendancesStudent, noAttendancesStudent)
                        });

                    }
                }
                else if (attendances != null && attendanceCollection.Order == GetMaxNumberByCourseType(attendanceCollection.CourseType))
                {
                    var noAttendancesStudent = attendances.Count(a => a.UserID.Equals(user.Key) && a.AttendanceCollection!.CourseType == attendanceCollection.CourseType);
                    //check if this is the last collection and the student don't have enough attendances 
                    if (noAttendancesStudent + 1 < GetMaxNumberByCourseType(attendanceCollection.CourseType))
                    {
                        _unitOfWork.NotificationRepository.AddAsync(new()
                        {
                            Priority = Domain.Enums.NotificationPriority.Alert,
                            UserID = user.Key,
                            CreatedOn = DateTime.Now,
                            IsRead = false,
                            Message = string.Format(NotificationMessages.LastCollectionNoAttendancesNotification,
                                _currentReport.CurrentReportInfo.Title, attendanceCollection.CourseType.ToString(), noAttendancesStudent)
                        });
                    }
                }
                else
                {
                    //notification: a new collection was added
                    _unitOfWork.NotificationRepository.AddAsync(new()
                    {
                        Priority = Domain.Enums.NotificationPriority.Warning,
                        UserID = user.Key,
                        CreatedOn = DateTime.Now,
                        IsRead = false,
                        Message = string.Format(NotificationMessages.CreateCollectionNotification, request.Username, _currentReport.CurrentReportInfo.Title)
                    });
                }
            }

            // Save the members 
            if (!await _unitOfWork.CommitAsync(students.Count()))
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrong_CreateReportNotificationInsert_Error);
            }

            return attendanceCollection.AttendanceCollectionID;
        }
        private int GetMaxNumberByCourseType(CourseType type)
            => type switch
            {
                CourseType.Laboratory => _currentReport.CurrentReportInfo.MaxNumberOfLaboratories,
                CourseType.Seminary => _currentReport.CurrentReportInfo.MaxNumberOfSeminaries,
                CourseType.Lecture => _currentReport.CurrentReportInfo.MaxNumberOfLectures,
                _ => 0
            };
    }
}
