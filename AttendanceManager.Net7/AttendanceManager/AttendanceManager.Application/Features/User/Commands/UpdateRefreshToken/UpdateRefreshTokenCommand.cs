using MediatR;

namespace AttendanceManager.Application.Features.User.Commands.UpdateRefreshToken
{
    public sealed class UpdateRefreshTokenCommand : IRequest
    {
        public required string Email { get; init; }
        public required string RefreshToken { get; init; }
        public required DateTime ExpRefreshToken { get; init; }

    }
}
