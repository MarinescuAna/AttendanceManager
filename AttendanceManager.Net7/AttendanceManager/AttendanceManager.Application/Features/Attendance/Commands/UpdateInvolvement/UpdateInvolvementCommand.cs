using AttendanceManager.Application.Contracts.Infrastructure.Rewards;
using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Core.Shared;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Attendance.Commands.UpdateInvolvement
{
    public sealed class UpdateInvolvementCommand : IRequest<bool>
    {
        public required int AttendanceId { get; init; }
        public int? AttendanceCollectionId { get; init; }
        public string? AttendanceCode { get; init; }
        public bool? IsPresent { get; init; }
        public int? BonusPoints { get; init; }
    }

    public sealed class UpdateInvolvementCommandHandler : BaseFeature, IRequestHandler<UpdateInvolvementCommand, bool>
    {
        private IRewardService _rewardService;
        public UpdateInvolvementCommandHandler(IUnitOfWork unit, IMapper mapper, IRewardService rewardService) : base(unit, mapper)
        {
            _rewardService = rewardService;
        }

        public async Task<bool> Handle(UpdateInvolvementCommand request, CancellationToken cancellationToken)
        {
            // get the involvement
            var objAttendance = await unitOfWork.AttendanceRepository.GetAsync(a => a.AttendanceID == request.AttendanceId)
                    ?? throw new NotFoundException("Attendance", request.AttendanceId);

            if (!string.IsNullOrEmpty(request.AttendanceCode) && request.AttendanceCollectionId != null)
            {
                // check if the code exists into the database
                var code = await unitOfWork.InvolvementCodeRepository.GetAsync(c => c.Code.Equals(request.AttendanceCode) && c.AttendanceCollectionId == request.AttendanceCollectionId)
                    ?? throw new NotFoundException("Code", request.AttendanceCode);

                // check if the code is still valid
                if (code.ExpirationDate.CompareTo(DateTime.Now) < 0)
                {
                    throw new SomethingWentWrongException("The code has expired!");
                }

                //update the involvement
                objAttendance.IsPresent = true;
            }
            else
            {
                objAttendance.IsPresent = (bool)request.IsPresent!;
                objAttendance.BonusPoints = (int)request.BonusPoints!;
            }

            objAttendance.UpdatedOn = DateTime.Now;
            unitOfWork.AttendanceRepository.Update(objAttendance);

            if (!await unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrongGenericMessage);
            }

            await _rewardService.AssignBadge(Domain.Enums.BadgeType.FirstAttendance, objAttendance.AttendanceCollection!, objAttendance.UserID);

            return true;
        }
    }
}
