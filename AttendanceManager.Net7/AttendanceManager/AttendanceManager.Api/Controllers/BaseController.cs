using AttendanceManager.Core.Shared;
using AttendanceManager.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AttendanceManager.Api.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly IMediator mediator;
        protected readonly IHttpContextAccessor httpContextAccessor;
        protected string UserEmail
        {
            get{
                var result = string.Empty;
                if (httpContextAccessor.HttpContext != null)
                {
                    result = HttpContext.User.FindFirstValue(ClaimTypes.Email)!;
                }
                return result;
            }
        }
        protected Role UserRole
        {
            get
            {
                var result = string.Empty;
                if (httpContextAccessor.HttpContext != null)
                {
                    result = HttpContext.User.FindFirstValue(ClaimTypes.Role)!;
                }
                return Enum.Parse<Role>(result);
            }
        }

        public BaseController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.mediator = mediator;

        }
    }
}
