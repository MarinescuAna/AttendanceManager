using AttendanceManager.Application.Features.Attendance.Commands.UpdateInvolvement;
using MediatR;

namespace AttendanceManager.Application.Features.Attendance.Commands.UpdateStudentsInvolvement
{
    public sealed class UpdateStudentsInvolvementCommand : IRequest<bool>
    {
        public required StudentInvolvementDto[] Involvements { get; init; }
    }

    public sealed class UpdateStudentsInvolvementCommandHandler : IRequestHandler<UpdateStudentsInvolvementCommand, bool>
    {
        private IMediator _mediator;
        public UpdateStudentsInvolvementCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<bool> Handle(UpdateStudentsInvolvementCommand request, CancellationToken cancellationToken)
        {
            //Update each student involvement separate because in this case, if the attendance is not udpated, the badge will not be received
            foreach (var student in request.Involvements)
            {
                await _mediator.Send(new UpdateInvolvementCommand()
                {
                    AttendanceId = student.AttendanceID,
                    BonusPoints = student.BonusPoints,
                    IsPresent = student.IsPresent,
                });
            }

            return true;
        }
    }
}
