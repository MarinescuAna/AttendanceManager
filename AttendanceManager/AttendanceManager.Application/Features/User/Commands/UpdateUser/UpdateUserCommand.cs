using MediatR;

namespace AttendanceManager.Application.Features.User.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest
    {
        public Domain.Entities.User User { get; set; }
    }
}
