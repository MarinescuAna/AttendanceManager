using AttendanceManager.Application.Features.Specialization.Commands.CreateSpecialization;
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
        /// Create a specialization
        /// </summary>
        /// <param name="specialization"></param>
        /// <returns>Success: return the name, departmentID and specializationID</returns>
        [HttpPost("create_specialization")]
        public async Task<IActionResult> CreateSpecialization([FromBody] CreateSpecializationCommand specialization)
        {
            return Ok(await mediator.Send(specialization));
        }
    }
}
