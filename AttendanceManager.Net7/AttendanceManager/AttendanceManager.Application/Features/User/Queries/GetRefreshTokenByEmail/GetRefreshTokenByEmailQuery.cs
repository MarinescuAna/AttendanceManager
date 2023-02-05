using MediatR;

namespace AttendanceManager.Application.Features.User.Queries.GetRefreshTokenByEmail
{
    public sealed class GetRefreshTokenByEmailQuery : IRequest<string?>
    {
        public required string Email { get; init; }
    }
}
