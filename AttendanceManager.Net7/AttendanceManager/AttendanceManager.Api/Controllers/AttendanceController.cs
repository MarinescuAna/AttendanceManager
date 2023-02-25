using AttendanceManager.Application.Features.Attendance.Commands.UpdateAttendances;
using AttendanceManager.Application.Features.Attendance.Queries.GetAttendanceByAttendanceCollectionID;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManager.Api.Controllers
{
    [Route("api/attendance"), ApiController, Authorize]
    public class AttendanceController : BaseController
    {
        public AttendanceController(IMediator mediator, IHttpContextAccessor httpContextAccessor) : base(mediator, httpContextAccessor)
        {
        }

        /// <summary>
        /// Get all attandances
        /// </summary>
        /// <returns>Success: list with all attendances</returns>
        [HttpGet("attendances_by_attendance_collection_id")]
        public async Task<IActionResult> GetAttendancesByAttendanceCollectionId(int attendanceCollectionId)
        {
            return Ok(await mediator.Send(new GetAttendanceByAttendanceCollectionIdQuery() { AttendanceCollectionId = attendanceCollectionId }));
        }

        /// <summary>
        /// Update the attendances
        /// </summary>
        /// <returns>Success: true/false</returns>
        [HttpPatch("update_attendances")]
        public async Task<IActionResult> UpdateAttendances([FromBody] UpdateAttendancesCommand command)
        {
            return Ok(await mediator.Send(command));
        }
    }
}
