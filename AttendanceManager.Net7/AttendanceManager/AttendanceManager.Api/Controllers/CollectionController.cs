using AttendanceManager.Application.Features.Collection.Commands.CreateCollection;
using AttendanceManager.Application.Features.Collection.Commands.DeleteCollectionById;
using AttendanceManager.Domain.Common;
using AttendanceManager.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace AttendanceManager.Api.Controllers
{
    [Route("api/collection"), ApiController, Authorize]
    public class CollectionController : BaseController
    {
        public CollectionController(IMediator mediator, IHttpContextAccessor httpContextAccessor) : base(mediator, httpContextAccessor)
        {
        }
        /// <summary>
        /// Create a new attendance collection
        /// <returns>true/false</returns>
        /// </summary>
        [HttpPost("create_collection")]
        public async Task<IActionResult> CreateCollectionAsync(string activityTime, string type)
        {

            if (string.IsNullOrWhiteSpace(activityTime) || string.IsNullOrWhiteSpace(type))
            {
                return BadRequest(ErrorMessages.BadRequest_CreateCollectionParams_Error);
            }

            if (!DateTime.TryParseExact(activityTime, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedActivityTime))
            {
                return BadRequest(ErrorMessages.BadRequest_CreateCollectionParams2_Error);
            }

            if (!Enum.TryParse(type, out CourseType courseType))
            {
                return BadRequest(ErrorMessages.BadRequest_CreateCollectionParams3_Error);
            }

            return Ok(await mediator.Send(new CreateCollectionCommand()
            {
                ActivityDateTime = parsedActivityTime,
                Username = Username,
                CourseType = courseType
            }));

        }

        [HttpDelete("delete/{collectionId:int}")]
        public async Task<IActionResult> DeleteCollection(int collectionId)
        {
            return Ok(await mediator.Send(new DeleteCollectionByIdCommand() { CollectionId = collectionId }));
        }

    }
}
