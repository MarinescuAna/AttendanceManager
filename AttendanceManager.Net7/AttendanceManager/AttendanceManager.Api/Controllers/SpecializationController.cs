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
        /// <returns>Success: list with all the specializations without any filter</returns>
        /// </summary>
        [HttpGet("specializations")]
        public async Task<IActionResult> GetSpecializations()
        {
            return Ok(await mediator.Send(new GetSpecializationsQuery()));
        }

        /// <summary>
        /// Create a specialization
        /// <returns>Success: true/false</returns>
        /// </summary>
        [HttpPost("create_specialization")]
        public async Task<IActionResult> CreateSpecialization([FromBody] CreateSpecializationCommand specialization)
        {
            return Ok(await mediator.Send(specialization));
        }
    }
}
