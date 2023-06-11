using AttendanceManager.Application.Features.Collection.Commands.CreateCollection;
using AttendanceManager.Application.Features.Collection.Commands.DeleteCollectionById;
using AttendanceManager.Domain.Common;
using AttendanceManager.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
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
        public async Task<IActionResult> CreateCollectionAsync([FromBody] CreateCollectionCommand command)
        {
            command.Username = Username;
            return Ok(await mediator.Send(command));
        }

        [HttpDelete("delete/{collectionId:int}")]
        public async Task<IActionResult> DeleteCollection(int collectionId)
        {
            return Ok(await mediator.Send(new DeleteCollectionByIdCommand() { CollectionId = collectionId }));
        }

    }
}
