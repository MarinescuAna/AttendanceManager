using AttendanceManager.Application.Features.User.Commands.CreateUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AttendanceManager.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    //TODO [Authorize]
    public class UserController : BaseController
    {
        public UserController(IMediator mediator):base(mediator) { }

        /// <summary>
        /// Create a user
        /// </summary>
        /// <param name="userCommand"></param>
        /// <returns>Success: ok</returns>
        [HttpPost("create_user")]
        public async Task<ActionResult> CreateUser([FromBody] CreateUserCommand userCommand)
        {
            return Ok(await mediator.Send(userCommand));
        }

    }
}
