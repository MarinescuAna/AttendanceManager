using AttendanceManager.Application.Features.Reward.Queries.GetAllRewardsByUserIdReportId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManager.Api.Controllers
{
    [Route("api/reward")]
    [ApiController]
    public class RewardsController : BaseController
    {
        public RewardsController(IMediator mediator, IHttpContextAccessor httpContextAccessor) : base(mediator, httpContextAccessor)
        {
        }

        [HttpGet("{reportId}")]
        public async Task<IActionResult> GetRewardsByReportId(int reportId)
        {
            return Ok(await mediator.Send(new GetAllRewardsByUserIdReportIdQuery() { Email = UserEmail, ReportId = reportId,Role = UserRole }));
        }
    }
}
