using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Application.Features.Reward.Commands.CreateReward;
using AttendanceManager.Core.Shared;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Attendance.Commands.UpdateStudentsInvolvement
{
    public sealed class UpdateStudentsInvolvementCommand : IRequest<bool>
    {
        public required int ReportId { get; init; }
        public required StudentInvolvementDto[] Involvements { get; init; }
    }

    public sealed class UpdateStudentsInvolvementCommandHandler :BaseDocumentFeature, IRequestHandler<UpdateStudentsInvolvementCommand, bool>
    {
        private IMediator _mediator;
        public UpdateStudentsInvolvementCommandHandler(IMediator mediator, IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
            _mediator = mediator;
        }

        public async Task<bool> Handle(UpdateStudentsInvolvementCommand request, CancellationToken cancellationToken)
        {
            //Update each student involvement separate because in this case, if the attendance is not udpated, the badge will not be received
            foreach (var student in request.Involvements)
            {
                var oldStudent = await unitOfWork.AttendanceRepository.GetAsync(a => a.AttendanceID == student.AttendanceID);

                if (oldStudent != null)
                {
                    oldStudent!.IsPresent = student.IsPresent;
                    oldStudent!.UpdatedOn = DateTime.Now;
                    oldStudent!.BonusPoints = student.BonusPoints;

                    unitOfWork.AttendanceRepository.Update(oldStudent);
                }
            }

            if(!await unitOfWork.CommitAsync(request.Involvements.Length))
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrongGenericMessage);
            }

            foreach(var involvment in request.Involvements)
            {
                await _mediator.Send(new CreateRewardCommand()
                {
                    BadgeType = Domain.Enums.BadgeType.FirstAttendance,
                    ReportId = request.ReportId,
                    UserId = involvment.UserId,
                    CommitChanges = false
                });
            }

            return await unitOfWork.CommitAsync();
        }
    }
}
