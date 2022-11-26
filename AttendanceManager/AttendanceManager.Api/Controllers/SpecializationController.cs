using AttendanceManager.Application.Features.Specialization.Commands.CreateSpecialization;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AttendanceManager.Api.Controllers
{
    [Route("api/specialization")]
    [ApiController]
    public class SpecializationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SpecializationController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("create_specialization")]
        public async Task<IActionResult> CreateSpecialization([FromBody] CreateSpecializationCommand specialization)
        {
            return Ok(await _mediator.Send(specialization));
        }
    }
}
