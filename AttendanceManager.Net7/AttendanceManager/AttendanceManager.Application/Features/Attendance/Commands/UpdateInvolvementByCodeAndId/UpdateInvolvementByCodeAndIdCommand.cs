using AttendanceManager.Application.Features.Attendance.Commands.UpdateInvolvement;
using MediatR;

namespace AttendanceManager.Application.Features.Attendance.Commands.UpdateInvolvementByCodeAndId
{
    public sealed class UpdateInvolvementByCodeAndIdCommand : IRequest<bool>
    {
        public required string AttendanceCode { get; init; }
        public required int AttendanceId { get; init; }
        public required int AttendanceCollectionId { get; init; }
    }

    public sealed class UpdateInvolvementByCodeAndIdCommandHandler : IRequestHandler<UpdateInvolvementByCodeAndIdCommand, bool>
    {
        private IMediator _mediator;
        public UpdateInvolvementByCodeAndIdCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<bool> Handle(UpdateInvolvementByCodeAndIdCommand request, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new UpdateInvolvementCommand()
            {
                AttendanceId = request.AttendanceId,
                AttendanceCode = request.AttendanceCode,
                AttendanceCollectionId = request.AttendanceCollectionId,
            });
        }
    }
}
