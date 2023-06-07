using AttendanceManager.Application.Features.Reward.Queries.GetAllRewardsByUserIdReportId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManager.Api.Controllers
{
    [Route("api/reward"), ApiController, Authorize]
    public class RewardsController : BaseController
    {
        public RewardsController(IMediator mediator, IHttpContextAccessor httpContextAccessor) : base(mediator, httpContextAccessor)
        {
        }

        [HttpGet("rewards")]
        public async Task<IActionResult> GetRewardsByReportId()
        {
            return Ok(await mediator.Send(new GetAllRewardsByUserIdReportIdQuery() { Email = UserEmail, Role = UserRole }));
        }
    }
}
