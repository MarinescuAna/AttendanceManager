using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AttendanceManager.Api.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly IMediator mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BaseController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            this.mediator = mediator;
        }

        protected string GetCurrentUserEmail()
        {
            var result = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = _httpContextAccessor.HttpContext.User.FindFirstValue("email")!;
            }
            return result;
        }
    }
}
