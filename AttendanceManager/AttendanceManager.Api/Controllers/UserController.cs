using AttendanceManager.Application.Features.User.Commands.CreateUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AttendanceManager.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    //[Authorize]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create_user")]
        public async Task<ActionResult> CreateUser([FromBody] CreateUserCommand userCommand)
        {
            return Ok(await _mediator.Send(userCommand));
        }

    }
}
