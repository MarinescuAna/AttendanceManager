using MediatR;

namespace AttendanceManager.Application.Features.User.Commands.UpdateUser
{
    public sealed class UpdateUserCommand : IRequest
    {
        public required Domain.Entities.User User { get; init; }
    }
}
