using MediatR;

namespace AttendanceManager.Application.Features.User.Queries.GetUserInformationByEmail
{
    public sealed class GetUserInformationByEmailQuery : IRequest<UserInfoDto>
    {
        public required string Email { get; init; }

    }
}
