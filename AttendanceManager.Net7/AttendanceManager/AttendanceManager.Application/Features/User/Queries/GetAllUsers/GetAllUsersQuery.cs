using MediatR;

namespace AttendanceManager.Application.Features.User.Queries.GetAllUsers
{
    public sealed class GetAllUsersQuery : IRequest<List<UserDto>>
    {
    }
}
