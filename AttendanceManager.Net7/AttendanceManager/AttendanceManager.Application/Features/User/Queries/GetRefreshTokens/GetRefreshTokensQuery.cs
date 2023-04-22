using MediatR;

namespace AttendanceManager.Application.Features.User.Queries.GetRefreshTokens
{
    public sealed class GetRefreshTokensQuery : IRequest<string[]?>
    {
    }
}
