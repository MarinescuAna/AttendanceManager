using AttendanceManager.Application.Features.Attendance.Queries.GetStudentAttendanceByDocIdAndUserId;
using AttendanceManager.Application.Features.Document.Commands.CreateDocument;
using AttendanceManager.Application.Features.Document.Queries.GetCreatedDocumentsByEmail;
using AttendanceManager.Application.Features.Document.Queries.GetDocumentById;
using AttendanceManager.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace AttendanceManager.Api.Controllers
{
    [Route("api/document"), ApiController, Authorize]
    public class DocumentController : BaseController
    {
        public DocumentController(IMediator mediator, IHttpContextAccessor httpContextAccessor) : base(mediator, httpContextAccessor)
        {
        }

        /// <summary>
        /// Get all documents according to the user email and role
        /// <returns>Success: the list with all created documents by the current user</returns>
        /// </summary>
        [HttpGet("documents")]
        public async Task<IActionResult> GetDocuments()
        {
            return Ok(await mediator.Send(new GetDocumentsQuery() { 
                Email = UserEmail,
                Role = UserRole
            }));
        }

        /// <summary>
        /// Get created document by the id
        /// <returns>Success: information regarding the document with the given id</returns>
        /// </summary>
        [HttpGet("document_by_id")]
        public async Task<IActionResult> GetDocumentById(int id)
        {
            var document = await mediator.Send(new GetDocumentByIdQuery(){ 
                Id = id,
                Role = UserRole
            });

            // load current student attendances instead of all members attendances in case that the user is student
            if (UserRole == Domain.Enums.Role.Student)
            {
                document.CurrentStudentAttendances = await mediator.Send(new GetStudentAttendanceByDocIdAndUserIdQuery()
                {
                    DocumentId = id,
                    UserId = UserEmail
                });
            }
            return Ok(document);
        }

        /// <summary>
        /// Create a new document
        /// <returns>Success: true/false</returns>
        /// </summary>
        [HttpPost("create_document")]
        public async Task<IActionResult> CreateDocument([FromBody] CreateDocumentCommand command)
        {
            command.Email = UserEmail;
            return Ok(await mediator.Send(command));
        }
    }
}
