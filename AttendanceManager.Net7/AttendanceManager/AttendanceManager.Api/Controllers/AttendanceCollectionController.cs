using AttendanceManager.Application.Features.AttendanceCollection.Commands.CreateAttendanceCollection;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManager.Api.Controllers
{
    [Route("api/attendance_collection"), ApiController, Authorize]
    public class AttendanceCollectionController : BaseController
    {
        public AttendanceCollectionController(IMediator mediator, IHttpContextAccessor httpContextAccessor) : base(mediator, httpContextAccessor)
        {
        }
        /// <summary>
        /// Create a new attendance collection
        /// <returns>true/false</returns>
        /// </summary>
        [HttpPost("create_attendance_collection")]
        public async Task<IActionResult> CreateAttendanceCollection([FromBody] CreateAttendanceCollectionCommand command)
        {
            return Ok(await mediator.Send(command));
        }

    }
}
