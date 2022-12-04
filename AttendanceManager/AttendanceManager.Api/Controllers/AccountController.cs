using AttendanceManager.Application.Contracts.Authentication;
using AttendanceManager.Application.Models.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AttendanceManager.Api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public AccountController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        /// <summary>
        /// Login functionality
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Success: token and refresh token</returns>
        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthenticationResponse>> AuthenticateAsync([FromBody] AuthenticationRequest request)
        {
            return Ok(await _authenticationService.AuthenticateAsync(request));
        }
        //TODO refresh token
    }
}
