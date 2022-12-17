using MediatR;

namespace AttendanceManager.Application.Features.User.Queries.GetUserByEmail
{
    public sealed class GetUserByEmailQuery : IRequest<UserDto>
    {
        public required string Email { get; set; }
    }
}
