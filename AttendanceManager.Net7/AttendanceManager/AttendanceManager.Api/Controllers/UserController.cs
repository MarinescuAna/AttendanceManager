﻿using AttendanceManager.Application.Features.User.Commands.CreateMultipleUsers;
using AttendanceManager.Application.Features.User.Commands.CreateUser;
using AttendanceManager.Application.Features.User.Queries.GetAllUsers;
using AttendanceManager.Application.Features.User.Queries.GetStudentsForCourses;
using AttendanceManager.Application.Features.User.Queries.GetUserInformationByEmail;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManager.Api.Controllers
{
    [Route("api/user"), ApiController, Authorize]
    public sealed class UserController : BaseController
    {
        public UserController(IMediator mediator, IHttpContextAccessor httpContextAccessor) : base(mediator, httpContextAccessor) { }

        /// <summary>
        /// Create a user
        /// </summary>
        /// <returns>Success: ok</returns>
        [HttpPost("create_user")]
        public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserCommand userCommand)
        {
            return Ok(await mediator.Send(userCommand));
        }
        [HttpPost("create_users")]
        public async Task<IActionResult> CreateUsersAsync([FromBody] CreateMultipleUsersCommand command)
        {
            return Ok(await mediator.Send(command));
        }

        /// <summary>
        /// Get all users
        /// <returns>Success: list with all the users</returns>
        /// </summary>
        [HttpGet("users")]
        public async Task<IActionResult> GetUsersAsync()
        {
            return Ok(await mediator.Send(new GetAllUsersQuery()));
        }

        /// <summary>
        /// Get all students by specialization and enrollment year
        /// <returns>Success: list with all the students(email and fullname)</returns>
        /// </summary>
        [HttpGet("students")]
        public async Task<IActionResult> GetStudentsBySpecializationIdEnrollmentYearAsync(int year, int specializationId)
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
        [HttpGet("current_user_info")]
        public async Task<IActionResult> GetCurrentUserInfoAsync()
        {
            return Ok(await mediator.Send(new GetUserInformationByEmailQuery() { Email = UserEmail }));
        }
    }
}
