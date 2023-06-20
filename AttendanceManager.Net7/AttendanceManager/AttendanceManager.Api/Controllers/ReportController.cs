using AttendanceManager.Application.Features.Report.Queries.GetReportById;
using AttendanceManager.Application.Features.Member.Commands.InsertCollaborator;
using AttendanceManager.Application.Features.Report.Commands.CreateReport;
using AttendanceManager.Application.Features.Report.Commands.DeleteReportById;
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

        [HttpGet("reports")]
        public async Task<IActionResult> GetReportsAsync()
        {
            return Ok(await mediator.Send(new GetReportsQuery()
            {
                Email = UserEmail,
                Role = UserRole,
            }));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetReportByIdAsync(int id)
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
        public async Task<IActionResult> AddCollaboratorAsync([FromBody] InsertCollaboratorCommand command)
        {
            command.CurrentEmail = UserEmail;
            command.CurrentUsername = Username;
            return Ok(await mediator.Send(command));
        }

        [HttpPatch("update_report")]
        public async Task<IActionResult> UpdateReportAsync([FromBody] UpdateReportByIdCommand command)
        {
            return Ok(await mediator.Send(command));
        }

        [HttpDelete("delete_current_report")]
        public async Task<IActionResult> DeleteReportAsync()
        {
            return Ok(await mediator.Send(new DeleteReportByIdCommand()));
        }


    }
}
