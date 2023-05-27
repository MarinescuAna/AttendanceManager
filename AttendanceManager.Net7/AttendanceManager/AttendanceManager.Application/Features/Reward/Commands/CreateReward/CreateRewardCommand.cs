using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Domain.Common;
using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;
using AutoMapper;
using MediatR;
using System.Collections.Immutable;

namespace AttendanceManager.Application.Features.Reward.Commands.CreateReward
{
    /// <summary>
    /// NOTE: FIRST INSERT THE BADGE, THEN INSERT THE ATTENDANCE!!!!!!!
    /// </summary>
    public sealed class CreateRewardCommand : IRequest<bool>
    {
        public required string UserId { get; init; }
        public required Role RoleRole { get; init; }
        public int CollectionId { get; init; }

        public bool CommitChanges { get; init; } = true;
    }
    public sealed class CreateRewardCommandHandler : BaseDocumentFeature, IRequestHandler<CreateRewardCommand, bool>
    {
        private IEnumerable<Domain.Entities.AttendanceCollection> _collections;
        public CreateRewardCommandHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public async Task<bool> Handle(CreateRewardCommand request, CancellationToken cancellationToken)
        {
            if (currentDocument == null)
            {
                throw new NoContentException(ErrorMessages.NoContentReportBaseMessage);
            }

            //get all the badges that the user have for the current reportID
            var activeBadges = await unitOfWork.RewardRepository.GetRewardsAsync(r => r.UserID == request.UserId && r.ReportID == currentDocument!.DocumentId);

            //get all the badges that are inactive
            var inactiveBadges = unitOfWork.BadgeRepository.ListAll()
                .AsEnumerable()
                .Where(b => b.UserRole == request.RoleRole)
                .Where(b => activeBadges.Count(ab => Enum.Equals(ab.BadgeID,b.BadgeID)) == 0);

            if (inactiveBadges.Count() == 0)
            {
                // all the available bagdes was achieved
                return true;
            }

            //get all the collections related to the document
            _collections = unitOfWork.AttendanceCollectionRepository.GetCollectionsByReportId(currentDocument!.DocumentId).ToImmutableList();

            //get a list with all the badges that can be inserted
            var achievedRewards = GetAllAchievedBadges(inactiveBadges, request.UserId, request.CollectionId);

            //insert the achieved rewards
            foreach (var reward in achievedRewards)
            {
                unitOfWork.RewardRepository.AddAsync(reward);
            }

            if (request.CommitChanges && !await unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrongInsertBadgeMessage);
            }

            return true;
        }

        private IEnumerable<Domain.Entities.Reward> GetAllAchievedBadges(IEnumerable<Badge> inactiveBadges, string email,int collectionId)
        {
            var achievedRewards = new List<Domain.Entities.Reward>();
            foreach (var badge in inactiveBadges)
            {
                if (CanInsertBadge(badge.BadgeID, email, collectionId))
                {
                    achievedRewards.Add(new()
                    {
                        BadgeID = badge.BadgeID,
                        ReportID = currentDocument!.DocumentId,
                        UserID = email
                    });
                }
            }

            return achievedRewards;
        }
        private bool CanInsertBadge(BadgeID badge, string email, int collectionId)
        {
            var currentCollection = _collections!.FirstOrDefault(ac => ac.AttendanceCollectionID == collectionId);
            return badge switch
            {
                /// In order to achieve this badge, at this moment, the user should have no attendance at any collection defined under this report
                BadgeID.FirstAttendance => _collections.Any(ac => ac.Attendances!.Count(a => a.IsPresent && a.UserID.Equals(email)) != 0),
                // check if the current collection attendance of given type, is the last attendance that can be receieved
                BadgeID.LastAttendance => currentCollection!.Order == GetMaxNumberByCourseType(currentCollection!.CourseType),
                _ => false
            };
        }
        private int GetMaxNumberByCourseType(CourseType type)
            => type switch
            {
                CourseType.Laboratory => currentDocument!.MaxNoLaboratories,
                CourseType.Seminary => currentDocument!.MaxNoSeminaries,
                CourseType.Lecture => currentDocument!.MaxNoLessons,
                _ => 0
            };
    }
}
