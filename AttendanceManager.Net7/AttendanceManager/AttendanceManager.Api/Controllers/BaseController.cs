using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AttendanceManager.Api.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly IMediator mediator;
        public BaseController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        protected string? GetCurrentUserEmail()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                return userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            }
            return string.Empty;
        }
    }
}
