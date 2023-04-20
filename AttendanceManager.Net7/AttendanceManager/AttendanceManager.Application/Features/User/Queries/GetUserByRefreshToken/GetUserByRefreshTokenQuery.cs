using AttendanceManager.Application.Features.User.Queries.Dtos;
using MediatR;

namespace AttendanceManager.Application.Features.User.Queries.GetUserByRefreshToken
{
    public sealed class GetUserByRefreshTokenQuery : IRequest<UserByRefreshTokenDto>
    {
        public required string RefreshToken { get; init; }
    }
}
