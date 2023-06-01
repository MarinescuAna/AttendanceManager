using AttendanceManager.Application.Features.Document.Commands.CreateDocument;
using AttendanceManager.Application.Features.Document.Commands.DeleteDocumentById;
using AttendanceManager.Application.Features.Document.Commands.UpdateDocumentById;
using AttendanceManager.Application.Features.Document.Queries.GetCreatedDocumentsByEmail;
using AttendanceManager.Application.Features.Document.Queries.GetReportById;
using AttendanceManager.Application.Features.DocumentMember.Commands.InsertCollaborator;
using AttendanceManager.Domain.Common;
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocumentById(int? id)
        {
            if (id == null)
            {
                return BadRequest(ErrorMessages.BadRequest_ParametersMissing_Error);
            }

            return Ok(await mediator.Send(new GetReportByIdQuery()
            {
                Id = (int)id,
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
        public async Task<IActionResult> AddCollaborator(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(ErrorMessages.BadRequest_ParametersMissing_Error);
            }

            if (email.Equals(UserEmail))
            {
                return BadRequest(ErrorMessages.BadRequest_InvalidParameters_Collaborator_Error);
            }

            return Ok(await mediator.Send(new InsertCollaboratorCommand()
            {
                Email = email,
                CurrentUsername = Username
            }));
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

        /// <summary>
        /// Delete (soft or hard ) a document
        /// </summary>
        /// <returns>Success: true/false</returns>
        [HttpDelete("delete_document/{id}")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            return Ok(await mediator.Send(new DeleteDocumentByIdCommand { DocumentId = id }));
        }


    }
}
