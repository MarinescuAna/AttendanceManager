using AttendanceManager.Application.Contracts.Infrastructure.Singleton;
using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Domain.Common;
using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;
using MediatR;
using System.Collections.Immutable;

namespace AttendanceManager.Application.Features.Reward.Commands.CreateReward
{
    /// <summary>
    /// NOTE: FIRST INSERT THE ATTENDANCE, THEN INSERT THE BADGES!!!!!!!
    /// </summary>
    public sealed class CreateRewardCommand : IRequest<bool>
    {
        public required string AchievedUserId { get; init; }
        public required Role AchievedUserRole { get; init; }
        public int CurrentCollectionId { get; init; }
        public BadgeType? BadgeID { get; init; } = null;

        public bool CommitChanges { get; init; } = true;
    }
    public sealed class CreateRewardCommandHandler : IRequestHandler<CreateRewardCommand, bool>
    {
        public readonly IUnitOfWork _unitOfWork;
        public IReportSingleton _currentReport;
        private IEnumerable<AttendanceCollection>? _collections;
        private readonly int _currentReportId;
        private CreateRewardCommand? _command;
        public CreateRewardCommandHandler(IUnitOfWork unit, IReportSingleton reportSingleton)
        {
            _unitOfWork = unit;
            _currentReport = reportSingleton;
            _currentReportId = reportSingleton.CurrentReportInfo.ReportId;

            if (_currentReport.CurrentReportInfo == null)
            {
                throw new Exceptions.NotImplementedException(ErrorMessages.NoContentReportBaseMessage);
            }
        }

        public async Task<bool> Handle(CreateRewardCommand request, CancellationToken cancellationToken)
        {
            _command = request;
            //get all the badges that are inactive
            var inactiveBadges = await _unitOfWork.BadgeRepository.GetUnachievedBadgesAsync(_command.AchievedUserId, _currentReportId, _command.AchievedUserRole);

            if (inactiveBadges.Count() == 0)
            {
                // all the available bagdes was achieved
                return true;
            }

            //get all the collections related to the document
            _collections = _unitOfWork.AttendanceCollectionRepository.GetCollectionsByReportId(_currentReportId).ToImmutableList();

            if (_collections == null)
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrong_CollectionsNotAvailableError);
            }

            //get a list with all the badges that can be inserted
            var achievedRewards = GetAllAchievedBadges(inactiveBadges);

            //insert the achieved rewards
            foreach (var reward in achievedRewards)
            {
                var badge = inactiveBadges.Find(b => b.BadgeID == reward.BadgeID);

                _unitOfWork.RewardRepository.AddAsync(reward);
                _unitOfWork.NotificationRepository.AddAsync(new()
                {
                    CreatedOn = DateTime.Now,
                    IsRead = false,
                    Message = string.Format(NotificationMessages.AchievedBadgeNotification, badge!.Title, _currentReport.CurrentReportInfo.Title),
                    Priority = NotificationPriority.Info,
                    UserID = _command.AchievedUserId,
                    Image = badge.ImagePath,
                });
            }

            if (achievedRewards.Count() > 0 && _command.CommitChanges && !await _unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrongInsertBadgeMessage);
            }

            return achievedRewards.Count() > 0;
        }

        private IEnumerable<Domain.Entities.Reward> GetAllAchievedBadges(IEnumerable<Domain.Entities.Badge> inactiveBadges)
        {
            var achievedRewards = new List<Domain.Entities.Reward>();
            var currentCollection = _collections!.FirstOrDefault(ac => ac.AttendanceCollectionID == _command!.CurrentCollectionId);

            foreach (var badge in inactiveBadges)
            {
                var result = _command!.AchievedUserRole.Equals(Role.Student) ? IsStudentBadgeAchieved(badge, currentCollection!) :
                    IsTeacherBadgeAchieved(badge, currentCollection!);

                if (result)
                {
                    achievedRewards.Add(new()
                    {
                        BadgeID = badge.BadgeID,
                        ReportID = _currentReportId,
                        UserID = _command!.AchievedUserId
                    });
                }
            }

            return achievedRewards;
        }
        private bool IsTeacherBadgeAchieved(Domain.Entities.Badge badge, AttendanceCollection currentCollection)
        {
            var isEqualsOrGreaterHalfCollection = IsHalfOrMoreHeld(currentCollection!.CourseType) &&
                GetMaxNumberByCourseType(currentCollection!.CourseType) != 0;
            var isLastCollection = GetMaxNumberByCourseType(currentCollection.CourseType) != 0 &&
                GetMaxNumberByCourseType(currentCollection.CourseType) == currentCollection.Order;
            var studentsAttendances = new Dictionary<string, int>();

            if (isEqualsOrGreaterHalfCollection && (badge.BadgeType.Equals(BadgeType.GoodTeacher) || badge.BadgeType.Equals(BadgeType.BestTeacher)))
            {
                studentsAttendances = ComputeStudentsAttendance(currentCollection.CourseType);
            }

            return badge.BadgeType switch
            {
                //achieve this when the first involvement code is generated
                BadgeType.FirstCodeGenerated => FirstCodeGenerated(currentCollection.AttendanceCollectionID),
                //achieve this when the first involvement code is used
                BadgeType.FirstCodeUsed => _command!.BadgeID != null && _command!.BadgeID == BadgeType.FirstCodeUsed,
                //this badge is received when all the students come to the one activity
                //check if all the students came to the lecture
                BadgeType.FullClass => currentCollection.Attendances!.Count(a => a.IsPresent) == _currentReport.CurrentReportInfo.NoOfStudents,
                BadgeType.CustomAttendanceAchieved => GetAttendancesAchievedByEmail((CourseType)badge.CourseType!) >= badge.MaxNumber,
                BadgeType.CustomBonusPointAchieved => GetBonusPointsAchievedByEmail((CourseType)badge.CourseType!) >= badge.MaxNumber,
                //more than half of the students come to the last laboratory
                BadgeType.SayByeLaboratory => isLastCollection && currentCollection.CourseType.Equals(CourseType.Laboratory) &&
                    currentCollection!.Attendances!.Count(a => a.IsPresent) >= GetMaxNumberByCourseType(CourseType.Laboratory),
                //more than half of the students come to the last lecture
                BadgeType.SayByeLecture => isLastCollection && currentCollection.CourseType.Equals(CourseType.Lecture) &&
                    currentCollection!.Attendances!.Count(a => a.IsPresent) >= GetMaxNumberByCourseType(CourseType.Lecture),
                //more than half of the students come to the seminary
                BadgeType.SayByeSeminary => isLastCollection && currentCollection.CourseType.Equals(CourseType.Seminary) &&
                    currentCollection!.Attendances!.Count(a => a.IsPresent) >= GetMaxNumberByCourseType(CourseType.Seminary),
                //the half of the students achieve attendance at any course
                BadgeType.GoodTeacher => studentsAttendances.Count() > 0 && studentsAttendances.Count(a => a.Value != 0) >= _currentReport.CurrentReportInfo.NoOfStudents/2,
                //this badge can be achieved when the half of the students achieve the more then half of attendance
                BadgeType.BestTeacher => studentsAttendances.Count() > 0 && studentsAttendances.Count(a => a.Value != 0) >= _currentReport.CurrentReportInfo.NoOfStudents/2 &&
                    studentsAttendances.Count(a=>a.Value >= GetMaxNumberByCourseType(currentCollection.CourseType)/2) >= _currentReport.CurrentReportInfo.NoOfStudents / 2,
                _ => false
            };
        }
        private bool IsStudentBadgeAchieved(Domain.Entities.Badge badge, AttendanceCollection currentCollection)
        {
            var isEqualsOrGreaterHalfCollection = IsHalfOrMoreHeld(currentCollection!.CourseType) &&
                GetMaxNumberByCourseType(currentCollection!.CourseType) != 0;
            var isCurrentActivityTypeComplete = IsCompleteHeld(currentCollection!.CourseType) && GetMaxNumberByCourseType(currentCollection!.CourseType) != 0;

            return badge.BadgeType switch
            {
                //achieved when a student gets the first attendance at any activity
                // In order to achieve this badge, at this moment, the user should have one attendance under this report
                BadgeType.FirstAttendance => currentCollection!.Attendances!.Any(a => a.UserID.Equals(_command!.AchievedUserId) && a.IsPresent),
                //this can be achieved by each student when they get the first bonus point 
                //check if this is the first bonus point achieved
                BadgeType.FirstBonus => currentCollection!.Attendances!.Any(a => a.UserID.Equals(_command!.AchievedUserId) && a.BonusPoints != 0),
                //– this will be achieved by the end of the lesson, laboratory or seminary, if the student has the grater number of bonus points 
                //check if the current if one of the users that have the maximum number of bonus points
                //it is important to know if all the collections was held or not
                BadgeType.SmartOwl => isCurrentActivityTypeComplete &&
                    GetAttendanceWithMaxBonusPoint(currentCollection.CourseType).Count(a => a.UserID.Equals(_command!.AchievedUserId)) != 0,
                //get last attendance to any of the available activities
                // get the collection that was completely held and check if the student have an attendance
                BadgeType.LastAttendance => isCurrentActivityTypeComplete &&
                    _collections!.Where(c => c.CourseType.Equals(currentCollection.CourseType)).Count(c => c.Attendances!.Any(a => a.UserID.Equals(_command!.AchievedUserId) && a.IsPresent)) == GetMaxNumberByCourseType(currentCollection.CourseType),

                //check if the current user, for given activity type, recevied the half of them
                //the student collects the half of the total attendances that was set by the teacher at lectures
                BadgeType.LecturesAttendances50 => currentCollection.CourseType == CourseType.Lecture && isEqualsOrGreaterHalfCollection &&
                    GetAttendancesAchievedByEmail(CourseType.Lecture) == GetMaxNumberByCourseType(CourseType.Lecture) / 2,
                //the student collects the half of the total attendances that was set by the teacher at laboratories
                BadgeType.LaboratoriesAttendances50 => currentCollection.CourseType == CourseType.Laboratory && isEqualsOrGreaterHalfCollection &&
                    GetAttendancesAchievedByEmail(CourseType.Laboratory) == GetMaxNumberByCourseType(CourseType.Laboratory) / 2,
                //the student collects the half of the total attendances that was set by the teacher at seminaries
                BadgeType.SeminariesAttendances50 => currentCollection.CourseType == CourseType.Seminary && isEqualsOrGreaterHalfCollection &&
                    GetAttendancesAchievedByEmail(CourseType.Seminary) == GetMaxNumberByCourseType(CourseType.Seminary) / 2,

                //check if the current user, for given activity type, recevied the all of them
                //achieved when the student collects all the attendances that was set by the teacher to lectures
                BadgeType.LecturesAttendancesComplete => currentCollection.CourseType == CourseType.Lecture && isCurrentActivityTypeComplete &&
                    GetAttendancesAchievedByEmail(CourseType.Lecture) == GetMaxNumberByCourseType(CourseType.Lecture) && isCurrentActivityTypeComplete,
                //achieved when the student collects all the attendances that was set by the teacher to laboratories
                BadgeType.LaboratoriesAttendancesComplete => currentCollection.CourseType == CourseType.Laboratory &&
                    GetAttendancesAchievedByEmail(CourseType.Laboratory) == GetMaxNumberByCourseType(CourseType.Laboratory) && isCurrentActivityTypeComplete,
                //achieved when the student collects all the attendances that was set by the teacher to seminaries
                BadgeType.SeminariesAttendancesComplete => currentCollection.CourseType == CourseType.Seminary &&
                    GetAttendancesAchievedByEmail(CourseType.Seminary) == GetMaxNumberByCourseType(CourseType.Seminary),
                _ => false

            };
        }

        private Dictionary<string, int> ComputeStudentsAttendance(CourseType courseType)
        {
            var result = new Dictionary<string, int>();
            var attendances = _collections!.Where(c => c.CourseType.Equals(courseType))
                .SelectMany(a => a.Attendances!.Where(a => a.IsPresent)).ToList();
            foreach (var student in _currentReport.Members.Where(a => a.Value.Equals(Role.Student)))
            {
                result.Add(student.Key, attendances.Count(a => a.UserID.Equals(student.Key) && a.IsPresent));
            }
            return result;
        }
        /// <summary>
        /// Get the course type of the type that have half of the collections
        /// </summary>
        /// <returns></returns>
        private bool IsHalfOrMoreHeld(CourseType courseType)
            => courseType switch
            {
                CourseType.Lecture => _currentReport.LastCollectionOrder[courseType] >= _currentReport.CurrentReportInfo.MaxNumberOfLectures / 2,
                CourseType.Laboratory => _currentReport.LastCollectionOrder[courseType] >= _currentReport.CurrentReportInfo.MaxNumberOfLaboratories / 2,
                CourseType.Seminary => _currentReport.LastCollectionOrder[courseType] >= _currentReport.CurrentReportInfo.MaxNumberOfSeminaries / 2,
                _ => false
            };
        /// <summary>
        /// Get the course type of the type that have all the collections
        /// </summary>
        /// <returns></returns>
        private bool IsCompleteHeld(CourseType courseType)
            => courseType switch
            {
                CourseType.Lecture => _currentReport.LastCollectionOrder[courseType] == _currentReport.CurrentReportInfo.MaxNumberOfLectures,
                CourseType.Laboratory => _currentReport.LastCollectionOrder[courseType] == _currentReport.CurrentReportInfo.MaxNumberOfLaboratories,
                CourseType.Seminary => _currentReport.LastCollectionOrder[courseType] == _currentReport.CurrentReportInfo.MaxNumberOfSeminaries,
                _ => false
            };
        private bool FirstCodeGenerated(int collectionID)
            => _unitOfWork.InvolvementCodeRepository.ListAll().Count(c => c.AttendanceCollectionId == collectionID) == 1;
        private int GetBonusPointsAchievedByEmail(CourseType type)
            => _collections!
                .Where(c => c.CourseType.Equals(type))
                .SelectMany(c => c.Attendances!)
                .Where(a => a.UserID.Equals(_command!.AchievedUserId))
                .Sum(a => a.BonusPoints);
        private int GetAttendancesAchievedByEmail(CourseType type)
            => _collections!.Count(c => c.Attendances!.Any(a => a.UserID.Equals(_command!.AchievedUserId) && a.IsPresent) && c.CourseType.Equals(type));
        private int GetMaxNumberByCourseType(CourseType type)
            => type switch
            {
                CourseType.Laboratory => _currentReport.CurrentReportInfo.MaxNumberOfLaboratories,
                CourseType.Seminary => _currentReport.CurrentReportInfo.MaxNumberOfSeminaries,
                CourseType.Lecture => _currentReport.CurrentReportInfo.MaxNumberOfLectures,
                _ => 0
            };
        private List<Domain.Entities.Attendance> GetAttendanceWithMaxBonusPoint(CourseType courseType)
        {
            var attendances = _collections!.Where(c => c.CourseType.Equals(courseType))
                .SelectMany(a => a.Attendances!.Where(a => a.IsPresent && a.BonusPoints != 0)).ToList();

            if (attendances.Count() == 0)
            {
                return new List<Domain.Entities.Attendance>();
            }

            var max = attendances.Select(a => a.BonusPoints).Max();
            return attendances.Where(a => a.BonusPoints == max).ToList();
        }
    }
}
