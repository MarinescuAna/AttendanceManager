using AttendanceManager.Application.Features.Document.Commands.CreateDocument;
using AttendanceManager.Application.Features.Document.Commands.DeleteDocumentById;
using AttendanceManager.Application.Features.Document.Commands.UpdateDocumentById;
using AttendanceManager.Application.Features.Document.Queries.GetCreatedDocumentsByEmail;
using AttendanceManager.Application.Features.Document.Queries.GetReportById;
using AttendanceManager.Application.Features.Member.Commands.InsertCollaborator;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            return Ok(await mediator.Send(new GetDocumentsQuery()
            {
                Email = UserEmail,
                Role = UserRole,
            }));
        }

        /// <summary>
        /// Get created document by the id
        /// <returns>Success: information regarding the document with the given id</returns>
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDocumentById(int id)
        {
            return Ok(await mediator.Send(new GetReportByIdQuery()
            {
                Id = id,
                Role = UserRole,
                UserId = UserEmail
            }));
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

        /// <summary>
        /// Add a user as collaborator to the document
        /// <returns>Success: true/false</returns>
        /// </summary>
        [HttpPost("add_collaborator")]
        public async Task<IActionResult> AddCollaborator([FromBody] InsertCollaboratorCommand command)
        {
            command.CurrentEmail = UserEmail;
            command.CurrentUsername = Username;
            return Ok(await mediator.Send(command));
        }

        /// <summary>
        /// Update the document information
        /// </summary>
        /// <returns>Success: true/false</returns>
        [HttpPatch("update_document")]
        public async Task<IActionResult> UpdateDocument([FromBody] UpdateDocumentByIdCommand command)
        {
            return Ok(await mediator.Send(command));
        }

        [HttpDelete("delete_current_report")]
        public async Task<IActionResult> DeleteDocument()
        {
            return Ok(await mediator.Send(new DeleteDocumentByIdCommand()));
        }


    }
}
