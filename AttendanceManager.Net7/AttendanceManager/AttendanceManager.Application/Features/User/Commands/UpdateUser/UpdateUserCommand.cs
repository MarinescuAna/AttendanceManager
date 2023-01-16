using MediatR;

namespace AttendanceManager.Application.Features.User.Commands.UpdateUser
{
    public sealed class UpdateUserCommand : IRequest
    {
        public required string Email { get; init; }
        public bool? Confirmed { get; init; }
    }
}
