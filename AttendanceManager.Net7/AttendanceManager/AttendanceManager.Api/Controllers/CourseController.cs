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
        /// Get all courses
        /// </summary>
        /// <returns>Success: list with all courses</returns>
        [HttpGet("courses")]
        public async Task<ActionResult<List<CoursesDto>>> GetCourses(string Email)
        {
            return Ok(await mediator.Send(new GetCoursesQuery() { Email = Email} ));
        }

        /// <summary>
        /// Create a new course
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Success: the guid of the new course</returns>
        [HttpPost("create_course")]
        public async Task<ActionResult> CreateCourse(CreateCourseCommand createCourseCommand)
        {
            return Ok(await mediator.Send(createCourseCommand));
        }

        /// <summary>
        /// Soft delete an course
        /// </summary>
        /// <param name="name"></param>
        [HttpPatch("delete_course")]
        public async Task<ActionResult> DeleteCourse(DeleteCourseCommand command)
        {
            return Ok(await mediator.Send(command));
        }

        /// <summary>
        /// Update the course name
        /// </summary>
        /// <param name="name"></param>
        [HttpPatch("update_course")]
        public async Task<ActionResult> UpdateCourse(UpdateCourseNameCommand command)
        {
            return Ok(await mediator.Send(command));
        }
    }
}
