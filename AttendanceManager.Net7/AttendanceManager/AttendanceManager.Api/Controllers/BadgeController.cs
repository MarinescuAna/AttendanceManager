using AttendanceManager.Application.Features.Badge.Commands.InsertCustomBadge;
using AttendanceManager.Application.Features.Badge.Queries.GetBadgePercentage;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManager.Api.Controllers
{
    [Route("api/badge"), ApiController, Authorize]
    public class BadgeController : BaseController
    {
        public BadgeController(IMediator mediator, IHttpContextAccessor httpContextAccessor) : base(mediator, httpContextAccessor)
        {
        }

        [HttpPost("create_badge")]
        public async Task<IActionResult> CreateBadgeAsync([FromBody] InsertCustomBadgeCommand command)
        {
            return Ok(await mediator.Send(command));
        }

        [HttpGet("badges")]
        public async Task<IActionResult> GetBadgesPercentageAsync()
        {
            return Ok(await mediator.Send(new GetBadgePercentageQuery()));
        }
    }
}
