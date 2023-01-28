using MediatR;

namespace AttendanceManager.Application.Features.User.Commands.ConfirmUserAccount
{
    public sealed class ConfirmUserAccountCommand : IRequest
    {
        public required string Email { get; init; }
    }
}
