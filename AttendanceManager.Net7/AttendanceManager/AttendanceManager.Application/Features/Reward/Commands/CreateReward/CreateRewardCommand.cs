﻿using AttendanceManager.Application.Contracts.Infrastructure.Singleton;
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
        public BadgeType? BadgeType { get; init; } = null;

        public bool CommitChanges { get; init; } = true;
    }
    public sealed class CreateRewardCommandHandler : IRequestHandler<CreateRewardCommand, bool>
    {
        public readonly IUnitOfWork _unitOfWork;
        public IReportSingleton _currentReport;
        private IEnumerable<Domain.Entities.Collection>? _collections;
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

            //get all the collections related to the report
            _collections = _unitOfWork.CollectionRepository.GetCollectionsByReportId(_currentReportId).ToImmutableList();

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

                await _unitOfWork.RewardRepository.AddAsync(reward);
                await _unitOfWork.NotificationRepository.AddAsync(new()
                {
                    CreatedOn = DateTime.Now,
                    IsRead = false,
                    Message = string.Format(NotificationMessages.AchievedBadgeNotification, badge!.Title, _currentReport.CurrentReportInfo.Title),
                    Priority = NotificationPriority.Info,
                    UserID = _command.AchievedUserId,
                    Image = badge.ImagePath,
                });
            }

            if (achievedRewards.Count() > 0 && _command.CommitChanges)
            {
                await _unitOfWork.CommitAsync();
            }

            return achievedRewards.Count() > 0;
        }

        private IEnumerable<Domain.Entities.Reward> GetAllAchievedBadges(IEnumerable<Domain.Entities.Badge> inactiveBadges)
        {
            var achievedRewards = new List<Domain.Entities.Reward>();
            var currentCollection = _collections!.FirstOrDefault(ac => ac.CollectionID == _command!.CurrentCollectionId);

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
        private bool IsTeacherBadgeAchieved(Domain.Entities.Badge badge, Domain.Entities.Collection currentCollection)
        {
            var maxNoOfClasses = GetMaxNumberByCourseType(currentCollection!.ActivityType);
            var isEqualsOrGreaterHalfCollection = IsHalfOrMoreHeld(currentCollection!.ActivityType) && maxNoOfClasses != 0;
            var isLastCollection = maxNoOfClasses != 0 && maxNoOfClasses == currentCollection.Order;
            var studentsAttendances = new Dictionary<string, int>();

            if (isEqualsOrGreaterHalfCollection && (badge.BadgeType.Equals(BadgeType.GoodTeacher) || badge.BadgeType.Equals(BadgeType.BestTeacher)))
            {
                studentsAttendances = ComputeStudentsAttendance(currentCollection.ActivityType);
            }

            return badge.BadgeType switch
            {
                //achieve this when the first involvement code is generated
                BadgeType.FirstCodeGenerated => FirstCodeGenerated(currentCollection.CollectionID),
                //achieve this when the first involvement code is used
                BadgeType.FirstCodeUsed => _command!.BadgeType != null && _command!.BadgeType == BadgeType.FirstCodeUsed,
                //this badge is received when all the students come to the one activity
                //check if all the students came to the lecture
                BadgeType.FullClass => currentCollection.Involvements!.Count(a => a.IsPresent) == _currentReport.CurrentReportInfo.NoOfStudents,
                //more than half of the students come to the last laboratory
                BadgeType.SayByeLaboratory => isLastCollection && currentCollection.ActivityType.Equals(ActivityType.Laboratory) &&
                    currentCollection!.Involvements!.Count(a => a.IsPresent) >= maxNoOfClasses,
                //more than half of the students come to the last lecture
                BadgeType.SayByeLecture => isLastCollection && currentCollection.ActivityType.Equals(ActivityType.Lecture) &&
                    currentCollection!.Involvements!.Count(a => a.IsPresent) >= maxNoOfClasses,
                //more than half of the students come to the seminary
                BadgeType.SayByeSeminary => isLastCollection && currentCollection.ActivityType.Equals(ActivityType.Seminary) &&
                    currentCollection!.Involvements!.Count(a => a.IsPresent) >= maxNoOfClasses,
                //the half of the students achieve attendance at any course
                BadgeType.GoodTeacher => studentsAttendances.Count() > 0 && studentsAttendances.Count(a => a.Value != 0) >= _currentReport.CurrentReportInfo.NoOfStudents/2,
                //this badge can be achieved when the half of the students achieve the more then half of attendance
                BadgeType.BestTeacher => studentsAttendances.Count() > 0 && studentsAttendances.Count(a => a.Value != 0) >= _currentReport.CurrentReportInfo.NoOfStudents/2 &&
                    studentsAttendances.Count(a=>a.Value >= maxNoOfClasses / 2) >= _currentReport.CurrentReportInfo.NoOfStudents / 2,
                _ => false
            };
        }
        private bool IsStudentBadgeAchieved(Domain.Entities.Badge badge, Domain.Entities.Collection currentCollection)
        {
            var isEqualsOrGreaterHalfCollection = IsHalfOrMoreHeld(currentCollection!.ActivityType) &&
                GetMaxNumberByCourseType(currentCollection!.ActivityType) != 0;
            var isCurrentActivityTypeComplete = IsCompleteHeld(currentCollection!.ActivityType) && GetMaxNumberByCourseType(currentCollection!.ActivityType) != 0;

            return badge.BadgeType switch
            {
                //achieved when a student gets the first attendance at any activity
                // In order to achieve this badge, at this moment, the user should have one attendance under this report
                BadgeType.FirstAttendance => currentCollection!.Involvements!.Any(a => a.UserID.Equals(_command!.AchievedUserId) && a.IsPresent),
                //this can be achieved by each student when they get the first bonus point 
                //check if this is the first bonus point achieved
                BadgeType.FirstBonus => currentCollection!.Involvements!.Any(a => a.UserID.Equals(_command!.AchievedUserId) && a.BonusPoints != 0),
                //– this will be achieved by the end of the lesson, laboratory or seminary, if the student has the grater number of bonus points 
                //check if the current if one of the users that have the maximum number of bonus points
                //it is important to know if all the collections was held or not
                BadgeType.SmartOwl => isCurrentActivityTypeComplete &&
                    GetAttendanceWithMaxBonusPoint(currentCollection.ActivityType).Count(a => a.UserID.Equals(_command!.AchievedUserId)) != 0,
                //get last attendance to any of the available activities
                // get the collection that was completely held and check if the student have an attendance
                BadgeType.LastAttendance => isCurrentActivityTypeComplete &&
                    _collections!.FirstOrDefault(c => c.ActivityType.Equals(currentCollection.ActivityType) && c.Order == GetMaxNumberByCourseType(currentCollection.ActivityType))
                    .Involvements.Any(i=>i.UserID.Equals(_command!.AchievedUserId) && i.IsPresent),

                //check if the current user, for given activity type, recevied the half of them
                //the student collects the half of the total attendances that was set by the teacher at lectures
                BadgeType.LecturesAttendances50 => currentCollection.ActivityType == ActivityType.Lecture && isEqualsOrGreaterHalfCollection &&
                    GetAttendancesAchievedByEmail(ActivityType.Lecture) == GetMaxNumberByCourseType(ActivityType.Lecture) / 2,
                //the student collects the half of the total attendances that was set by the teacher at laboratories
                BadgeType.LaboratoriesAttendances50 => currentCollection.ActivityType == ActivityType.Laboratory && isEqualsOrGreaterHalfCollection &&
                    GetAttendancesAchievedByEmail(ActivityType.Laboratory) == GetMaxNumberByCourseType(ActivityType.Laboratory) / 2,
                //the student collects the half of the total attendances that was set by the teacher at seminaries
                BadgeType.SeminariesAttendances50 => currentCollection.ActivityType == ActivityType.Seminary && isEqualsOrGreaterHalfCollection &&
                    GetAttendancesAchievedByEmail(ActivityType.Seminary) == GetMaxNumberByCourseType(ActivityType.Seminary) / 2,

                //check if the current user, for given activity type, recevied the all of them
                //achieved when the student collects all the attendances that was set by the teacher to lectures
                BadgeType.LecturesAttendancesComplete => currentCollection.ActivityType == ActivityType.Lecture && isCurrentActivityTypeComplete &&
                    GetAttendancesAchievedByEmail(ActivityType.Lecture) == GetMaxNumberByCourseType(ActivityType.Lecture) && isCurrentActivityTypeComplete,
                //achieved when the student collects all the attendances that was set by the teacher to laboratories
                BadgeType.LaboratoriesAttendancesComplete => currentCollection.ActivityType == ActivityType.Laboratory &&
                    GetAttendancesAchievedByEmail(ActivityType.Laboratory) == GetMaxNumberByCourseType(ActivityType.Laboratory) && isCurrentActivityTypeComplete,
                //achieved when the student collects all the attendances that was set by the teacher to seminaries
                BadgeType.SeminariesAttendancesComplete => currentCollection.ActivityType == ActivityType.Seminary &&
                    GetAttendancesAchievedByEmail(ActivityType.Seminary) == GetMaxNumberByCourseType(ActivityType.Seminary),

                BadgeType.CustomAttendanceAchieved => GetAttendancesAchievedByEmail(currentCollection.ActivityType) == badge.MaxNumber,
                BadgeType.CustomBonusPointAchieved => GetBonusPointsAchievedByEmail(currentCollection.ActivityType) == badge.MaxNumber,
                _ => false

            };
        }

        private Dictionary<string, int> ComputeStudentsAttendance(ActivityType courseType)
        {
            var result = new Dictionary<string, int>();
            var attendances = _collections!.Where(c => c.ActivityType.Equals(courseType))
                .SelectMany(a => a.Involvements!.Where(a => a.IsPresent)).ToList();
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
        private bool IsHalfOrMoreHeld(ActivityType courseType)
            => courseType switch
            {
                ActivityType.Lecture => _currentReport.LastCollectionOrder[courseType] >= _currentReport.CurrentReportInfo.MaxNumberOfLectures / 2,
                ActivityType.Laboratory => _currentReport.LastCollectionOrder[courseType] >= _currentReport.CurrentReportInfo.MaxNumberOfLaboratories / 2,
                ActivityType.Seminary => _currentReport.LastCollectionOrder[courseType] >= _currentReport.CurrentReportInfo.MaxNumberOfSeminaries / 2,
                _ => false
            };
        /// <summary>
        /// Get the course type of the type that have all the collections
        /// </summary>
        /// <returns></returns>
        private bool IsCompleteHeld(ActivityType courseType)
            => courseType switch
            {
                ActivityType.Lecture => _currentReport.LastCollectionOrder[courseType] == _currentReport.CurrentReportInfo.MaxNumberOfLectures,
                ActivityType.Laboratory => _currentReport.LastCollectionOrder[courseType] == _currentReport.CurrentReportInfo.MaxNumberOfLaboratories,
                ActivityType.Seminary => _currentReport.LastCollectionOrder[courseType] == _currentReport.CurrentReportInfo.MaxNumberOfSeminaries,
                _ => false
            };
        private bool FirstCodeGenerated(int collectionID)
            => _unitOfWork.InvolvementCodeRepository.ListAll().Count(c => c.CollectionID == collectionID) == 1;
        private int GetBonusPointsAchievedByEmail(ActivityType type)
            => _collections!
                .Where(c => c.ActivityType.Equals(type))
                .SelectMany(c => c.Involvements!)
                .Where(a => a.UserID.Equals(_command!.AchievedUserId))
                .Sum(a => a.BonusPoints);
        private int GetAttendancesAchievedByEmail(ActivityType type)
            => _collections!.Count(c => c.Involvements!.Any(a => a.UserID.Equals(_command!.AchievedUserId) && a.IsPresent) && c.ActivityType.Equals(type));
        private int GetMaxNumberByCourseType(ActivityType type)
            => type switch
            {
                ActivityType.Laboratory => _currentReport.CurrentReportInfo.MaxNumberOfLaboratories,
                ActivityType.Seminary => _currentReport.CurrentReportInfo.MaxNumberOfSeminaries,
                ActivityType.Lecture => _currentReport.CurrentReportInfo.MaxNumberOfLectures,
                _ => 0
            };
        private List<Domain.Entities.Involvement> GetAttendanceWithMaxBonusPoint(ActivityType courseType)
        {
            var attendances = _collections!.Where(c => c.ActivityType.Equals(courseType))
                .SelectMany(a => a.Involvements!.Where(a => a.IsPresent && a.BonusPoints != 0)).ToList();

            if (attendances.Count() == 0)
            {
                return new List<Domain.Entities.Involvement>();
            }

            var max = attendances.Select(a => a.BonusPoints).Max();
            return attendances.Where(a => a.BonusPoints == max).ToList();
        }
    }
}
