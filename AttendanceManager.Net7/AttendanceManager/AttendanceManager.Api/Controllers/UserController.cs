using AttendanceManager.Application.Features.User.Commands.CreateUser;
using AttendanceManager.Application.Features.User.Queries.GetAllUsers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManager.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    //TODO [Authorize]
    public sealed class UserController : BaseController
    {
        public UserController(IMediator mediator) : base(mediator) { }

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

        /// <summary>
        /// Get all users for admin
        /// </summary>
        /// <returns>list with the informations about all users</returns>
        [HttpGet("users")]
        public async Task<ActionResult> GetUsers()
        {
            return Ok(await mediator.Send(new GetAllUsersQuery()));
        }

    }
}
