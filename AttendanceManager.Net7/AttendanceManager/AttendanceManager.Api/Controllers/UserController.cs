using AttendanceManager.Application.Features.User.Commands.CreateUser;
using AttendanceManager.Application.Features.User.Queries.GetAllUsers;
using AttendanceManager.Application.Features.User.Queries.GetStudentsForCourses;
using AttendanceManager.Application.Features.User.Queries.GetUserInformationByEmail;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManager.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    //TODO [Authorize]
    public sealed class UserController : BaseController
    {
        public UserController(IMediator mediator) : base(mediator) { }

        /// <summary>
        /// Create a user
        /// </summary>
        /// <param name="userCommand"></param>
        /// <returns>Success: ok</returns>
        [HttpPost("create_user")]
        public async Task<ActionResult> CreateUser([FromBody] CreateUserCommand userCommand)
        {
            return Ok(await mediator.Send(userCommand));
        }

        /// <summary>
        /// Get all users for admin
        /// </summary>
        [HttpGet("users")]
        public async Task<ActionResult> GetUsers()
        {
            return Ok(await mediator.Send(new GetAllUsersQuery()));
        }

        /// <summary>
        /// Get all users for admin
        /// </summary>
        [HttpGet("students_by_specializationId_enrollmentYear")]
        public async Task<ActionResult> GetStudentsBySpecializationIdEnrollmentYear(string year, string specializationId)
        {
            return Ok(await mediator.Send(new GetStudentsForCoursesQuery() { 
            EnrollmentYear= year,
            SpecializationId  = specializationId
            }));
        }

        /// <summary>
        /// Get additional information about current user
        /// </summary>
        // TODO remove this email from here
        [HttpGet("current_user_info")]
        public async Task<ActionResult> GetCurrentUserInfo(string email)
        {
            return Ok(await mediator.Send(new GetUserInformationByEmailQuery() { Email = email}));
        }
    }
}
