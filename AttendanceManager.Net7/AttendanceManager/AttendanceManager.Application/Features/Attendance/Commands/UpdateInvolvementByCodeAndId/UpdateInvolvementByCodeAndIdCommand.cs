using AttendanceManager.Application.Contracts.Infrastructure.Singleton;
using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Application.Features.Reward.Commands.CreateReward;
using AttendanceManager.Domain.Common;
using MediatR;

namespace AttendanceManager.Application.Features.Attendance.Commands.UpdateInvolvementByCodeAndId
{
    public sealed class UpdateInvolvementByCodeAndIdCommand : IRequest<bool>
    {
        public required string Code { get; init; }
        public required int AttendanceId { get; init; }
        public required int CollectionId { get; init; }
    }

    public sealed class UpdateInvolvementByCodeAndIdCommandHandler : IRequestHandler<UpdateInvolvementByCodeAndIdCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReportSingleton _currentReport;
        private readonly IMediator _mediator;

        public UpdateInvolvementByCodeAndIdCommandHandler(IUnitOfWork unit, IReportSingleton reportSingleton, IMediator mediator)
        {
            _currentReport = reportSingleton;
            _unitOfWork = unit;
            _mediator = mediator;

            if (_currentReport.CurrentReportInfo == null)
            {
                throw new Exceptions.NotImplementedException(ErrorMessages.NoContentReportBaseMessage);
            }
        }

        public async Task<bool> Handle(UpdateInvolvementByCodeAndIdCommand request, CancellationToken cancellationToken)
        {
            // check if the code exists into the database
            var code = await _unitOfWork.InvolvementCodeRepository.GetAsync(c => c.Code.Equals(request.Code) && c.CollectionID == request.CollectionId)
                ?? throw new NotFoundException("Code", request.Code);

            // check if the code is still valid
            if (code.ExpirationDate.CompareTo(DateTime.Now) < 0)
            {
                throw new SomethingWentWrongException("The code has expired!");
            }

            // get the attendance
            var attendance = await _unitOfWork.AttendanceRepository.GetAsync(a => a.AttendanceID.Equals(request.AttendanceId))
                ?? throw new NotFoundException("Attendance", request.AttendanceId);

            //update the attendance
            attendance.IsPresent = true;
            _unitOfWork.AttendanceRepository.Update(attendance);

            if(!await _unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrongGenericMessage);
            }

            await _mediator.Send(new CreateRewardCommand()
            {
                CurrentCollectionId = request.CollectionId,
                AchievedUserRole = Domain.Enums.Role.Teacher,
                AchievedUserId = _currentReport.CurrentReportInfo.CreatedBy,
                BadgeID=Domain.Enums.BadgeType.FirstCodeUsed,
                CommitChanges = true
            });

            return true;
        }

    }
}
