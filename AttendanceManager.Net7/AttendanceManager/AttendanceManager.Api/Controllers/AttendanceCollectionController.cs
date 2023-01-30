using AttendanceManager.Application.Features.AttendanceCollection.Commands.CreateAttendanceCollection;
using AttendanceManager.Application.Features.AttendanceCollection.Queries.GetAttendanceCollectionByDocumentId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManager.Api.Controllers
{
    [Route("api/attendance_collection"), ApiController, Authorize]
    public class AttendanceCollectionController : BaseController
    {
        public AttendanceCollectionController(IMediator mediator, IHttpContextAccessor httpContextAccessor) : base(mediator, httpContextAccessor)
        {
        }
        /// <summary>
        /// Create a new attendance collection
        /// <returns>true/false</returns>
        /// </summary>
        [HttpPost("create_attendance_collection")]
        public async Task<IActionResult> CreateAttendanceCollection([FromBody] CreateAttendanceCollectionCommand command)
        {
            return Ok(await mediator.Send(command));
        }
        /// <summary>
        /// Get all courses
        /// </summary>
        /// <returns>Success: list with all attendance collections</returns>
        [HttpGet("attendance_collection_by_documentId")]
        public async Task<IActionResult> GetAttendanceCollectionByDocumentId(int documentId)
        {
            return Ok(await mediator.Send(new GetAttendanceCollectionByDocumentIdQuery() { DocumentId = documentId }));
        }

    }
}
