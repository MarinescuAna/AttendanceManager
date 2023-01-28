using AttendanceManager.Application.Features.Course.Commands.CreateCourse;
using AttendanceManager.Application.Features.Course.Commands.DeleteCourse;
using AttendanceManager.Application.Features.Course.Commands.UpdateCourseName;
using AttendanceManager.Application.Features.Course.Queries.GetCoursesQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManager.Api.Controllers
{
    [Route("api/course")]
    [ApiController]
    //TODO [Authorize]
    public class CourseController : BaseController
    {
        public CourseController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Get all courses related to the current user
        /// </summary>
        /// <returns>Success: list with all courses</returns>
        [HttpGet("courses")]
        public async Task<IActionResult> GetCourses(string Email)
        {
            //TODO change this: remove the params email
            //return Ok(await mediator.Send(new GetCoursesQuery() { Email = GetCurrentUserEmail()} ));
            return Ok(await mediator.Send(new GetCoursesQuery() { Email = Email }));
        }

        /// <summary>
        /// Create a new course. This action can be done only by the teacher
        /// </summary>
        /// <returns>Success: the id of the new course</returns>
        [HttpPost("create_course")]
        public async Task<IActionResult> CreateCourse(CreateCourseCommand createCourseCommand)
        {
            return Ok(await mediator.Send(createCourseCommand));
        }

        /// <summary>
        /// Soft delete an course
        /// <returns>Success: boolean value</returns>
        /// </summary>
        [HttpDelete("delete_course")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            return Ok(await mediator.Send(new DeleteCourseCommand { Id = id }));
        }

        /// <summary>
        /// Update the course name
        /// <returns>Success: boolean value</returns>
        /// </summary>        
        [HttpPatch("update_course")]
        public async Task<IActionResult> UpdateCourse(UpdateCourseNameCommand command)
        {
            return Ok(await mediator.Send(command));
        }
    }
}
