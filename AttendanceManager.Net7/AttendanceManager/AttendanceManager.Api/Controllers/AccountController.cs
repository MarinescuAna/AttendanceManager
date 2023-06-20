using AttendanceManager.Application.Contracts.Infrastructure.Authentication;
using AttendanceManager.Application.Features.User.Queries.GetUserByRefreshToken;
using AttendanceManager.Application.Models.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManager.Api.Controllers
{
    [Route("api/account"), ApiController, AllowAnonymous]
    public sealed class AccountController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IJsonWebTokenService _jwtService;
        public AccountController(IMediator mediator, IAuthenticationService authenticationService, IHttpContextAccessor httpContextAccessor, IJsonWebTokenService jwtService)
            : base(mediator, httpContextAccessor)
        {
            _authenticationService = authenticationService;
            _jwtService = jwtService;
        }
        /// <summary>
        /// Login functionality
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Success: token and refresh token</returns>
        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync([FromBody] AuthenticationRequest request)
        {
            return Ok(await _authenticationService.AuthenticateAsync(request));
        }

        /// <summary>
        /// Method used for generating the access token
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Success: new access token</returns>
        [HttpPost("refresh-access-token")]
        public async Task<IActionResult> RefreshAccessTokenAsync(string refreshToken)
        {
            var user = await mediator.Send(new GetUserByRefreshTokenQuery { RefreshToken = refreshToken});

            if (user == null || string.IsNullOrEmpty(user.RefreshToken))
            {
                return Unauthorized("Refresh token is missing!");
            }

            if (!user.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Invalid refresh token!");
            }

            var accesstoken = _jwtService.GenerateAccessToken(user.Email, user.FullName, user.Role, user.Code);

            return Ok(accesstoken);
        }
    }

}
