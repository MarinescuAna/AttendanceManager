using MediatR;
using System.Collections.Generic;

namespace AttendanceManager.Application.Features.User.Queries.GetAllUsers
{
    public class GetAllUsersQuery : IRequest<List<UserDto>>
    {
    }
}
