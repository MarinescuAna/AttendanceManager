using AttendanceManager.Application.Features.InvolvementCode.Commands.CreateInvolvementCode;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManager.Api.Controllers
{
    [Route("api/involvement_code"), ApiController, Authorize]
    public class InvolvementCodeController : BaseController
    {
        public InvolvementCodeController(IMediator mediator, IHttpContextAccessor httpContextAccessor) : base(mediator, httpContextAccessor)
        {
        }

        /// <summary>
        /// Create a new involvement code
        /// <returns>return the code and the expiration date</returns>
        /// </summary>
        [HttpPost("create_code/{minutes}/{attendanceCollectionId}")]
        public async Task<IActionResult> CreateInvolvmentCode(int minutes, int attendanceCollectionId)
        {
            return Ok(await mediator.Send(new CreateInvolvementCodeCommand()
            {
                Minutes = minutes,
                AttendanceCollectionId = attendanceCollectionId
            }));
        }
    }
}
