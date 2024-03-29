﻿using AttendanceManager.Application.Features.Course.Commands.CreateCourse;
using AttendanceManager.Application.Features.Course.Commands.DeleteCourse;
using AttendanceManager.Application.Features.Course.Commands.UpdateCourseName;
using AttendanceManager.Application.Features.Course.Queries.GetCoursesQuery;
using AttendanceManager.Application.Features.Course.Queries.GetDashboardDataQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManager.Api.Controllers
{
    [Route("api/course"), ApiController, Authorize]
    public class CourseController : BaseController
    {
        public CourseController(IMediator mediator, IHttpContextAccessor httpContextAccessor) : base(mediator, httpContextAccessor)
        {
        }

        /// <summary>
        /// Get all courses related to the current user
        /// </summary>
        /// <returns>Success: list with all courses</returns>
        [HttpGet("courses")]
        public async Task<IActionResult> GetCoursesAsync()
        {
            return Ok(await mediator.Send(new GetCoursesQuery() { Email = UserEmail } ));
        }

        /// <summary>
        /// Create a new course. This action can be done only by the teacher
        /// </summary>
        /// <returns>Success: the id of the new course</returns>
        [HttpPost("create_course")]
        public async Task<IActionResult> CreateCourseAsync(CreateCourseCommand createCourseCommand)
        {
            createCourseCommand.Email = UserEmail;
            return Ok(await mediator.Send(createCourseCommand));
        }

        /// <summary>
        /// Soft delete an course
        /// <returns>Success: boolean value</returns>
        /// </summary>
        [HttpDelete("delete_course/{id:int}")]
        public async Task<IActionResult> DeleteCourseAsync(int id)
        {
            return Ok(await mediator.Send(new DeleteCourseCommand { Id = id }));
        }
       
        [HttpPatch("update_course")]
        public async Task<IActionResult> UpdateCourseAsync(UpdateCourseNameCommand command)
        {
            command.UserEmail = UserEmail;
            return Ok(await mediator.Send(command));
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboardDataAsync()
        {
            return Ok(await mediator.Send(new GetDashboardDataQuery() { CurrentUserEmail = UserEmail }));
        }
    }
}
