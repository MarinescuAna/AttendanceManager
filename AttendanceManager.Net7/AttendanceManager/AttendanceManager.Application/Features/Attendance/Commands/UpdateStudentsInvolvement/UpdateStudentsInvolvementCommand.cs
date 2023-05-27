using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Application.Features.Reward.Commands.CreateReward;
using AttendanceManager.Domain.Enums;
using AttendanceManager.Domain.Common;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Attendance.Commands.UpdateStudentsInvolvement
{
    public sealed class UpdateStudentsInvolvementCommand : IRequest<bool>
    {
        public required StudentInvolvementVm[] Involvements { get; init; }
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
            if(currentDocument== null) {
                throw new NoContentException(ErrorMessages.NoContentReportBaseMessage);
            }

            //Update each student involvement separate because in this case, if the attendance is not udpated, the badge will not be received
            foreach (var student in request.Involvements)
            {
                var oldStudent = await unitOfWork.AttendanceRepository.GetAsync(a => a.AttendanceID == student.InvolvementId);

                if (oldStudent != null)
                {
                    oldStudent!.IsPresent = student.IsPresent;
                    oldStudent!.UpdatedOn = DateTime.Now;
                    oldStudent!.BonusPoints = student.BonusPoints;

                    unitOfWork.AttendanceRepository.Update(oldStudent);
                }
            }

            foreach(var involvment in request.Involvements)
            {
                await _mediator.Send(new CreateRewardCommand()
                {
                    RoleRole = Role.Student,
                    UserId = involvment.UserId,
                    CollectionId = involvment.CollectionId,
                    CommitChanges = false
                });
            }

            return await unitOfWork.CommitAsync();
        }
    }
}
