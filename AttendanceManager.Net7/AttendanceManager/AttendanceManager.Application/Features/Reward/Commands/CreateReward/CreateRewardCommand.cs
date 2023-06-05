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

            return true;
        }

        private IEnumerable<Domain.Entities.Reward> GetAllAchievedBadges(IEnumerable<Domain.Entities.Badge> inactiveBadges)
        {
            var achievedRewards = new List<Domain.Entities.Reward>();
            var currentCollection = _collections!.FirstOrDefault(ac => ac.AttendanceCollectionID == _command!.CurrentCollectionId);

            foreach (var badge in inactiveBadges)
            {
                if (IsBadgeAchieved(badge, currentCollection!))
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
        private bool IsBadgeAchieved(Domain.Entities.Badge badge, AttendanceCollection currentCollection)
        {
            return badge.BadgeType switch
            {
                /// In order to achieve this badge, at this moment, the user should have one attendance under this report
                BadgeType.FirstAttendance => _collections!.Any(ac => ac.Attendances!.Count(a => a.IsPresent && a.UserID.Equals(_command!.AchievedUserId)) == 1),
                // get the collection, check if is the last collection of given type and check if the student have an attendance
                BadgeType.LastAttendance => currentCollection!.Order == GetMaxNumberByCourseType(currentCollection!.CourseType)
                                            && currentCollection.Attendances!.FirstOrDefault(a => a.UserID.Equals(_command!.AchievedUserId) && a.IsPresent) != null,
                //check if the current user, for given activity type, recevied the half of them
                BadgeType.LecturesAttendances50 => GetMaxNumberByCourseType(CourseType.Lecture) != 0 &&
                    GetAttendancesAchievedByEmail(CourseType.Lecture) == GetMaxNumberByCourseType(CourseType.Lecture) / 2,
                BadgeType.LaboratoriesAttendances50 => GetMaxNumberByCourseType(CourseType.Laboratory) != 0 &&
                    GetAttendancesAchievedByEmail(CourseType.Laboratory) == GetMaxNumberByCourseType(CourseType.Laboratory) / 2,
                BadgeType.SeminariesAttendances50 => GetMaxNumberByCourseType(CourseType.Seminary) != 0 &&
                    GetAttendancesAchievedByEmail(CourseType.Seminary) == GetMaxNumberByCourseType(CourseType.Seminary) / 2,
                //check if the current user, for given activity type, recevied the all of them
                BadgeType.LecturesAttendancesComplete => GetMaxNumberByCourseType(CourseType.Lecture) != 0 &&
                    GetAttendancesAchievedByEmail(CourseType.Lecture) == GetMaxNumberByCourseType(CourseType.Lecture),
                BadgeType.LaboratoriesAttendancesComplete => GetMaxNumberByCourseType(CourseType.Laboratory) != 0 &&
                    GetAttendancesAchievedByEmail(CourseType.Laboratory) == GetMaxNumberByCourseType(CourseType.Laboratory),
                BadgeType.SeminariesAttendancesComplete => GetMaxNumberByCourseType(CourseType.Seminary) != 0 &&
                    GetAttendancesAchievedByEmail(CourseType.Seminary) == GetMaxNumberByCourseType(CourseType.Seminary),
                //check if this is the first bonus point achieved
                BadgeType.FirstBonus => currentCollection!.Attendances!.Any(a => a.UserID.Equals(_command!.AchievedUserId) && a.BonusPoints != 0),
                //check if the current if one of the users that have the maximum number of bonus points
                BadgeType.SmartOwl => GetMaxNumberByCourseType(currentCollection.CourseType)==currentCollection.Order 
                     && GetAttendanceWithMaxBonusPoint().Count(a => a.UserID.Equals(_command!.AchievedUserId)) != 0,
                BadgeType.FirstCodeGenerated => FirstCodeGenerated(currentCollection.AttendanceCollectionID),
                BadgeType.FirstCodeUsed=>_command!.BadgeID!=null && _command!.BadgeID==BadgeType.FirstCodeUsed,
                //check if all the students came to the lecture
                BadgeType.FullClass =>currentCollection.Attendances!.Count(a=>a.IsPresent) == _currentReport.CurrentReportInfo.NoOfStudents,
                BadgeType.CustomAttendanceAchieved => GetAttendancesAchievedByEmail((CourseType)badge.CourseType!)>=badge.MaxNumber,
                BadgeType.CustomBonusPointAchieved => GetBonusPointsAchievedByEmail((CourseType)badge.CourseType!)>=badge.MaxNumber,
                _ => false
            };
        }
        private bool FirstCodeGenerated(int collectionID)
            => _unitOfWork.InvolvementCodeRepository.ListAll().Count(c => c.AttendanceCollectionId == collectionID) == 1;
        private int GetBonusPointsAchievedByEmail(CourseType type)
            => _collections!
                .Where(c=>c.CourseType.Equals(type))
                .SelectMany(c => c.Attendances!)
                .Where(a=>a.UserID.Equals(_command!.AchievedUserId))
                .Sum(a=>a.BonusPoints);
        private int GetAttendancesAchievedByEmail( CourseType type)
            => _collections!.Count(c => c.Attendances!.Any(a => a.UserID.Equals(_command!.AchievedUserId) && a.IsPresent) && c.CourseType.Equals(type));
        private int GetMaxNumberByCourseType(CourseType type)
            => type switch
            {
                CourseType.Laboratory => _currentReport.CurrentReportInfo.MaxNumberOfLaboratories,
                CourseType.Seminary => _currentReport.CurrentReportInfo.MaxNumberOfSeminaries,
                CourseType.Lecture => _currentReport.CurrentReportInfo.MaxNumberOfLectures,
                _ => 0
            };
        private List<Domain.Entities.Attendance> GetAttendanceWithMaxBonusPoint()
        {
            var attendances = _collections!.SelectMany(a => a.Attendances!.Where(a => a.IsPresent && a.BonusPoints != 0)).ToList();

            if (attendances.Count() == 0)
            {
                return new List<Domain.Entities.Attendance>();
            }

            var max = attendances.Select(a => a.BonusPoints).Max();
            return attendances.Where(a => a.BonusPoints == max).ToList();
        }
    }
}
