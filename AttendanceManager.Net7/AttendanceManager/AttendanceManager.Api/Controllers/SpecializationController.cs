using AttendanceManager.Application.Features.Specialization.Commands.CreateSpecialization;
using AttendanceManager.Application.Features.Specialization.Queries.GetSpecializations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManager.Api.Controllers
{
    [Route("api/specialization")]
    [ApiController]
    //TODO [Authorize]
    public sealed class SpecializationController : BaseController
    {
        public SpecializationController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Get all specializations 
        /// </summary>
        [HttpGet("specializations")]
        public async Task<ActionResult<List<SpecializationDto>>> GetSpecializations()
        {
            return Ok(await mediator.Send(new GetSpecializationsQuery()));
        }

        /// <summary>
        /// Create a specialization
        /// </summary>
        [HttpPost("create_specialization")]
        public async Task<IActionResult> CreateSpecialization([FromBody] CreateSpecializationCommand specialization)
        {
            return Ok(await mediator.Send(specialization));
        }
    }
}
