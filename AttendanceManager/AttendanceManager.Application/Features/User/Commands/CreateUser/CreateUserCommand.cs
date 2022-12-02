using AttendanceManager.Domain.Enums;
using MediatR;

namespace AttendanceManager.Application.Features.User.Commands.CreateUser
{
    public class CreateUserCommand : IRequest
    {
        public string Email { get; set; }
        public string Role { get; set; }
        public string Fullname { get; set; }
        public string Year { get; set; }
        public string Code { get; set; }
    }
}
