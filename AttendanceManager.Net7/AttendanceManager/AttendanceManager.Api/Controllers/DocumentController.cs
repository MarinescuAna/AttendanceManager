using AttendanceManager.Application.Features.Document.Commands.CreateDocument;
using AttendanceManager.Application.Features.Document.Queries.GetCreatedDocumentsByEmail;
using AttendanceManager.Application.Features.Document.Queries.GetDocumentById;
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
        /// Get all created documents 
        /// <returns>Success: the list with all created documents by the current user</returns>
        /// </summary>
        [HttpGet("created_documents_by_email")]
        public async Task<IActionResult> GetCreatedDocuemntsByUserEmail()
        {
            return Ok(await mediator.Send(new GetCreatedDocumentsByEmailQuery() { Email = UserEmail }));
        }

        /// <summary>
        /// Get created document by the id
        /// <returns>Success: information regarding the document with the given id</returns>
        /// </summary>
        [HttpGet("document_by_id")]
        public async Task<IActionResult> GetDocumentById(int id)
        {
            return Ok(await mediator.Send(new GetDocumentByIdQuery() { Id = id }));
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
