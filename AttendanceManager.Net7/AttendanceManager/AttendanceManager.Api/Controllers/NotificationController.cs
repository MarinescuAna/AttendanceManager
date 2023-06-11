using AttendanceManager.Application.Features.Notification.Commands.DeleteNotificationById;
using AttendanceManager.Application.Features.Notification.Commands.ReadNotificationById;
using AttendanceManager.Application.Features.Notification.Queries.GetNotificationsByUserId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManager.Api.Controllers
{
    [Route("api/notification"), ApiController, Authorize]
    public class NotificationController : BaseController
    {
        public NotificationController(IMediator mediator, IHttpContextAccessor httpContextAccessor) : base(mediator, httpContextAccessor)
        {
        }

        [HttpGet("notifications")]
        public async Task<IActionResult> GetNotificationsAsync()
        {
            return Ok(await mediator.Send(new GetNotificationsByUserIdQuery() { UserId = UserEmail }));
        }

        [HttpPatch("read_message/{id:int}")]
        public async Task<IActionResult> ReadMessageAsync(int id)
        {
            return Ok(await mediator.Send(new ReadNotificationByIdCommand() { NotificationId = id }));
        }

        [HttpDelete("delete_notification/{id:int}")]
        public async Task<IActionResult> DeleteNotificationAsync(int id)
        {
            return Ok(await mediator.Send(new DeleteNotificationByIdCommand() { NotificationId = id }));
        }
    }
}
