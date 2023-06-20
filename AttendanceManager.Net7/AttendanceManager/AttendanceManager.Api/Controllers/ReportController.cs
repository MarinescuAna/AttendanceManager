using AttendanceManager.Application.Features.Document.Queries.GetReportById;
using AttendanceManager.Application.Features.Member.Commands.InsertCollaborator;
using AttendanceManager.Application.Features.Report.Commands.CreateReport;
using AttendanceManager.Application.Features.Report.Commands.DeleteDocumentById;
using AttendanceManager.Application.Features.Report.Commands.UpdateReportById;
using AttendanceManager.Application.Features.Report.Queries.GetReports;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManager.Api.Controllers
{
    [Route("api/report"), ApiController, Authorize]
    public class ReportController : BaseController
    {
        public ReportController(IMediator mediator, IHttpContextAccessor httpContextAccessor) : base(mediator, httpContextAccessor)
        {
        }

        /// <summary>
        /// Get all documents according to the user email and role
        /// <returns>Success: the list with all created documents by the current user</returns>
        /// </summary>
        [HttpGet("reports")]
        public async Task<IActionResult> GetReports()
        {
            return Ok(await mediator.Send(new GetReportsQuery()
            {
                Email = UserEmail,
                Role = UserRole,
            }));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetReportById(int id)
        {
            return Ok(await mediator.Send(new GetReportByIdQuery()
            {
                Id = id,
                Role = UserRole,
                UserId = UserEmail
            }));
        }

        [HttpPost("create_report")]
        public async Task<IActionResult> CreateReportAsync([FromBody] CreateReportCommand command)
        {
            command.Email = UserEmail;
            return Ok(await mediator.Send(command));
        }

        [HttpPost("add_collaborator")]
        public async Task<IActionResult> AddCollaborator([FromBody] InsertCollaboratorCommand command)
        {
            command.CurrentEmail = UserEmail;
            command.CurrentUsername = Username;
            return Ok(await mediator.Send(command));
        }

        [HttpPatch("update_report")]
        public async Task<IActionResult> UpdateReport([FromBody] UpdateReportByIdCommand command)
        {
            return Ok(await mediator.Send(command));
        }

        [HttpDelete("delete_current_report")]
        public async Task<IActionResult> DeleteReport()
        {
            return Ok(await mediator.Send(new DeleteReportByIdCommand()));
        }


    }
}
