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
        /// <returns>Success: ok</returns>
        [HttpPost("create_user")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand userCommand)
        {
            return Ok(await mediator.Send(userCommand));
        }

        /// <summary>
        /// Get all users
        /// <returns>Success: list with all the users</returns>
        /// </summary>
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await mediator.Send(new GetAllUsersQuery()));
        }

        /// <summary>
        /// Get all students by specialization and enrollment year
        /// <returns>Success: list with all the students(email and fullname)</returns>
        /// </summary>
        [HttpGet("students_by_specializationId_enrollmentYear")]
        public async Task<IActionResult> GetStudentsBySpecializationIdEnrollmentYear(int year, int specializationId)
        {
            return Ok(await mediator.Send(new GetStudentsForCoursesQuery()
            {
                EnrollmentYear = year,
                SpecializationId = specializationId
            }));
        }

        /// <summary>
        /// Get additional information about current user
        /// <returns>Success: user information</returns>
        /// </summary>
        // TODO remove this email from here
        [HttpGet("current_user_info")]
        public async Task<IActionResult> GetCurrentUserInfo(string email)
        {
            //return Ok(await mediator.Send(new GetUserInformationByEmailQuery() { Email = GetCurrentUserEmail() }));
            return Ok(await mediator.Send(new GetUserInformationByEmailQuery() { Email = email }));
        }
    }
}
