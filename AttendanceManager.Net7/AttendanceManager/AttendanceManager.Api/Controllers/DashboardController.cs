using AttendanceManager.Application.Features.Dashboard.Queries.GetDashboardForDocumentById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManager.Api.Controllers
{
    [Route("api/dashboard"), ApiController, Authorize]
    public class DashboardController : BaseController
    {
        public DashboardController(IMediator mediator, IHttpContextAccessor httpContextAccessor) : base(mediator, httpContextAccessor)
        {
        }
        /// <summary>
        /// Load data related to document dashboard
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("document_dashboard/{id}")]
        public async Task<IActionResult> GetDocumentDashboardAsync(int id)
        {
            return Ok(await mediator.Send(new GetDashboardForDocumentByIdQuery() { DocumentId = id }));
        }
    }
}
