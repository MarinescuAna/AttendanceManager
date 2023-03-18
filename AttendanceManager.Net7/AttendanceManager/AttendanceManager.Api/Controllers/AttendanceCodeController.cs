using AttendanceManager.Application.Features.AttendanceCode.Commands.CreateAttendanceCode;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManager.Api.Controllers
{
    [Route("api/attendance_code"), ApiController, Authorize]
    public class AttendanceCodeController : BaseController
    {
        public AttendanceCodeController(IMediator mediator, IHttpContextAccessor httpContextAccessor) : base(mediator, httpContextAccessor)
        {
        }

        /// <summary>
        /// Create a new attendance code
        /// <returns>return the code</returns>
        /// </summary>
        [HttpPost("create_attendance_code")]
        public async Task<IActionResult> CreateAttendanceCode(int minutes)
        {
            return Ok(await mediator.Send(new CreateAttendanceCodeCommand()
            {
                Minutes = minutes
            }));
        }
    }
}
