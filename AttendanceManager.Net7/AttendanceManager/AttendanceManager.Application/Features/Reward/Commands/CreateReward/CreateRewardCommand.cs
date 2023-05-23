using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Domain.Common;
using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Reward.Commands.CreateReward
{
    /// <summary>
    /// NOTE: FIRST INSERT THE BADGE, THEN INSERT THE ATTENDANCE!!!!!!!
    /// </summary>
    public sealed class CreateRewardCommand : IRequest<bool>
    {
        public required string UserId { get; init; }
        public required Role RoleRole { get; init; }

        public bool CommitChanges { get; init; } = true;
    }
    public sealed class CreateRewardCommandHandler : BaseDocumentFeature, IRequestHandler<CreateRewardCommand, bool>
    {
        public CreateRewardCommandHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public async Task<bool> Handle(CreateRewardCommand request, CancellationToken cancellationToken)
        {
            if(currentDocument == null)
            {
                throw new NoContentException(ErrorMessages.NoContentReportBaseMessage);
            }

            //get all the badges that the user have for the current reportID
            var activeBadges = await unitOfWork.RewardRepository.GetRewardsAsync(r => r.UserID == request.UserId && r.ReportID == currentDocument!.DocumentId);

            //get all the badges that are inactive
            var inactiveBadges = (await unitOfWork.BadgeRepository.ListAllAsync())
                .Where(b=>b.UserRole==request.RoleRole)
                .Where(b => activeBadges.Count(ab => ab.BadgeID == b.BadgeID)==0);

            if (inactiveBadges.Count() == 0)
            {
                // all the available bagdes was achieved
                return true;
            }

            //get a list with all the badges that can be inserted
            var achievedRewards = GetAllAchievedBadges(inactiveBadges,request.UserId);

            //insert the achieved rewards
            foreach(var reward in achievedRewards)
            {
                unitOfWork.RewardRepository.AddAsync(reward);
            }

            if (request.CommitChanges && !await unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrongInsertBadgeMessage);
            }

            return true;
        }

        private List<Domain.Entities.Reward> GetAllAchievedBadges(IEnumerable<Badge> inactiveBadges, string email)
        {
            var achievedRewards = new List<Domain.Entities.Reward>();
            foreach (var badge in inactiveBadges)
            {
                switch (badge.BadgeType)
                {
                    case BadgeType.FirstAttendance:
                        if (CanAchieveFirstAttendanceBadge(email))
                        {
                            achievedRewards.Add(new()
                            {
                                BadgeID = badge.BadgeID,
                                ReportID = currentDocument!.DocumentId,
                                UserID = email
                            });
                        }
                        break;
                    default:
                        break;
                }
            }

            return achievedRewards;
        }
        /// <summary>
        /// In order to achieve this badge, at this moment, the user should have no attendance at any collection defined under this report.
        /// This method checks if there is any attendance that have one of the report collections
        /// </summary>
        /// <returns>true if the badge can be achieved</returns>
        private bool CanAchieveFirstAttendanceBadge(string email) => 
            !unitOfWork.AttendanceCollectionRepository.HasAttendanceByReportIdUserId(currentDocument!.DocumentId, email);
    }
}
