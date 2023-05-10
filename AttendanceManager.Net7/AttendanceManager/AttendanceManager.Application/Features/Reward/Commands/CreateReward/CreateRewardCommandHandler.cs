using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Core.Shared;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Reward.Commands.CreateReward
{
    public sealed class CreateRewardCommandHandler : BaseFeature, IRequestHandler<CreateRewardCommand, bool>
    {
        public CreateRewardCommandHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public async Task<bool> Handle(CreateRewardCommand request, CancellationToken cancellationToken)
        {
            // Get the badge
            var badge = await unitOfWork.BadgeRepository.GetAsync(b => b.BadgeType == request.BadgeType)
                ?? throw new NotFoundException($"There is no badge with the type {request.BadgeType}");

            // Check if the badge is already added
            if (await unitOfWork.RewardRepository.GetAsync(c => c.UserID == request.UserId && c.ReportID == request.ReportId && c.BadgeID == badge.BadgeID) != null)
            {
                throw new AlreadyExistsException("Badge", request.BadgeType.ToString());
            }

            unitOfWork.RewardRepository.AddAsync(new() { 
                BadgeID = badge.BadgeID,
                ReportID= request.ReportId,
                UserID= request.UserId
            });

            if (!await unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(Constants.SomethingWentWrongMessage);
            }

            return true;
        }
    }
}
