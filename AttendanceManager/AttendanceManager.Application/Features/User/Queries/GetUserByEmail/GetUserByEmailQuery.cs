using MediatR;

namespace AttendanceManager.Application.Features.User.Queries.GetUserByEmail
{
    /// <summary>
    /// This query will return a user by email.
    /// UserVm = represents the type of the response.
    /// Email = represents "the parameter of the query"
    /// </summary>
    public class GetUserByEmailQuery : IRequest<UserVm>
    {
        public string Email { get; set; }
    }
}
