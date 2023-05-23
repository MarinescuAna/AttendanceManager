using AttendanceManager.Application.Features.Attendance.Commands.UpdateInvolvementByCodeAndId;
using AttendanceManager.Application.Features.Attendance.Commands.UpdateStudentsInvolvement;
using AttendanceManager.Application.Features.Attendance.Queries.GetAttendanceByAttendanceCollectionID;
using AttendanceManager.Application.Features.Attendance.Queries.GetInvolvementsByReportId;
using AttendanceManager.Application.Features.Attendance.Queries.GetStudentAttendanceByUserId;
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
        /// Get all the involvements for the given report
        /// </summary>
        /// <returns>Success: list with all involvements</returns>
        [HttpGet("involvements")]
        public async Task<IActionResult> GetInvolvementsForDocument()
        {
            return Ok(await mediator.Send(new GetInvolvementsByReportIdQuery()));
        }

        /// <summary>
        /// Get all attandances
        /// </summary>
        /// <returns>Success: list with all attendances</returns>
        [HttpGet("attendances_by_attendance_collection_id")]
        public async Task<IActionResult> GetAttendancesByAttendanceCollectionId(int attendanceCollectionId)
        {
            return Ok(await mediator.Send(new GetAttendanceByAttendanceCollectionIdQuery()
            {
                AttendanceCollectionId = attendanceCollectionId,
                Role = UserRole
            }));
        }

        /// <summary>
        /// Use this API to update the list of students involvement
        /// </summary>
        /// <returns>Success: true/false</returns>
        [HttpPatch("update_student_involvement")]
        public async Task<IActionResult> UpdateStudentsInvolvement([FromBody] UpdateStudentsInvolvementCommand command)
        {
            return Ok(await mediator.Send(command));
        }

        /// <summary>
        /// Update the involvement by code
        /// </summary>
        /// <returns>Success: true/false</returns>
        [HttpPatch("update_involvement_by_code")]
        public async Task<IActionResult> UpdateAttendanceByCode([FromBody] UpdateInvolvementByCodeAndIdCommand command)
        {
            return Ok(await mediator.Send(command));
        }

        /// <summary>
        /// Get all attandances
        /// </summary>
        /// <returns>Success: list with all attendances</returns>
        [HttpGet("student_attendances/{email}")]
        public async Task<IActionResult> GetStudentAttendancesByUserId(string email, bool isCurrentUser)
        {
            return Ok(await mediator.Send(new GetStudentAttendanceByUserIdQuery() { UserId = isCurrentUser ? UserEmail : email }));
        }
    }
}
