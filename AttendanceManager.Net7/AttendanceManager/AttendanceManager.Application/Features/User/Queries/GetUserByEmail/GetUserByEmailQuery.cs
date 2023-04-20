using MediatR;

namespace AttendanceManager.Application.Features.User.Queries.GetUserByEmail
{
    public sealed class GetUserByEmailQuery : IRequest<Dtos.UserByEmailDto>
    {
        public required string Email { get; init; }
    }
}
