using MediatR;

namespace AttendanceManager.Application.Features.User.Queries.GetUserByEmail
{
    public class GetUserByEmailQuery : IRequest<UserDto>
    {
        public string Email { get; set; }
    }
}
