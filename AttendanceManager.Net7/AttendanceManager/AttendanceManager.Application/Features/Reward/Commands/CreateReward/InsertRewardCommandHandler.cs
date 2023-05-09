using AttendanceManager.Application.Contracts.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Core.Shared;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Reward.Commands.CreateReward
{
    public sealed class InsertRewardCommandHandler : BaseFeature, IRequestHandler<InsertRewardCommand, bool>
    {
        public InsertRewardCommandHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public async Task<bool> Handle(InsertRewardCommand request, CancellationToken cancellationToken)
        {
            // Check if the badge is already added
            if (await unitOfWork.RewardRepository.GetAsync(c => c.UserID == request.UserId && c.ReportID == request.ReportId && c.BadgeID == request.BadgeId) != null)
            {
                throw new AlreadyExistsException("Badge", request.BadgeId.ToString());
            }

            unitOfWork.RewardRepository.AddAsync(new() { 
                BadgeID = request.BadgeId,
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
