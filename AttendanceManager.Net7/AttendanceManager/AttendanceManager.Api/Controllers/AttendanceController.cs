using AttendanceManager.Application.Exceptions;
using AttendanceManager.Application.Features.Attendance.Commands.UpdateInvolvementByCodeAndId;
using AttendanceManager.Application.Features.Attendance.Commands.UpdateStudentsInvolvement;
using AttendanceManager.Application.Features.Attendance.Queries.GetInvolvementsByReportId;
using AttendanceManager.Application.Features.Attendance.Queries.GetSumInvolvementsPerReport;
using AttendanceManager.Domain.Common;
using AttendanceManager.Domain.Enums;
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
        /// Get all the involvements for the given report by filter
        ///  - no filter -> will get all the involvements without any filter
        ///  - only_present -> get all the involvements where isPresent is true
        ///  - current_user -> get all the current user's involvements
        ///  - use_code -> hide email and take user's code instead
        ///  - email -> get the involvements for this user
        ///  - collection_id -> get involvements that has this collection id
        /// </summary>
        /// <returns>Success: list with all involvements</returns>
        [HttpGet("involvements")]
        public async Task<IActionResult> GetInvolvementsForReportAsync(string? email, int? collection_id, bool use_code = false, bool current_user = false, bool only_present=false)
        {
            if((!string.IsNullOrEmpty(email) || current_user) && collection_id != -1)
            {
                throw new BadRequestException(ErrorMessages.BadRequest_InvalidParameters_EmailCollection_Error);
            }

            var emailUsed = string.Empty;

            if (current_user)
            {
                emailUsed = UserEmail;
            }

            if (!string.IsNullOrEmpty(email))
            {
                emailUsed = email;
            }

            return Ok(await mediator.Send(new GetInvolvementsByReportIdQuery()
            {
                UseCode = use_code,
                CollectionId = collection_id,
                UserEmail = emailUsed,
                OnlyPresent = only_present
            }));
        }
        /// <summary>
        /// Get for each user, the total involvements 
        /// </summary>
        [HttpGet("total_involvements")]
        public async Task<IActionResult> GetTotalInvolvementsAsync()
        {
            if (!Enum.Equals(UserRole, Role.Teacher))
            {
                throw new UnauthorizedException(ErrorMessages.BadRequest_UnauthorizedError);
            }

            return Ok(await mediator.Send(new GetSumInvolvementsPerReportQuery()));
        }

        /// <summary>
        /// Use this API to update the list of students involvement
        /// </summary>
        /// <returns>Success: true/false</returns>
        [HttpPatch("update_student_involvement")]
        public async Task<IActionResult> UpdateStudentsInvolvementAsync([FromBody] UpdateStudentsInvolvementCommand command)
        {
            if (command.Involvements.Length == 0)
            {
                throw new BadRequestException(ErrorMessages.BadRequest_ParametersMissing_Error);
            }

            command.CurrentUserEmail = UserEmail;

            return Ok(await mediator.Send(command));
        }

        /// <summary>
        /// Update the involvement by code
        /// </summary>
        /// <returns>Success: true/false</returns>
        [HttpPatch("update_involvement_by_code")]
        public async Task<IActionResult> UpdateAttendanceByCodeAsync([FromBody] UpdateInvolvementByCodeAndIdCommand command)
        {
            return Ok(await mediator.Send(command));
        }
    }
}
