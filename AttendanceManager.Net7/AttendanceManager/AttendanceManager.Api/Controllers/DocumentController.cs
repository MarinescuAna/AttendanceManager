using AttendanceManager.Application.Features.Document.Commands.CreateDocument;
using AttendanceManager.Application.Features.Document.Queries.GetCreatedDocumentsByEmail;
using AttendanceManager.Application.Features.Document.Queries.GetDocumentById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManager.Api.Controllers
{
    [Route("api/document")]
    [ApiController]
    public class DocumentController : BaseController
    {
        public DocumentController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Get all created documents 
        /// </summary>
        [HttpGet("created_documents_by_email")]
        public async Task<IActionResult> GetCreatedDocuemntsByUserEmail(string email)
        {
            return Ok(await mediator.Send(new GetCreatedDocumentsByEmailQuery() { Email = email }));
        }

        /// <summary>
        /// Get all created documents 
        /// </summary>
        [HttpGet("document_by_id")]
        public async Task<IActionResult> GetDocumentById(string id)
        {
            return Ok(await mediator.Send(new GetDocumentByIdQuery() { Id = id }));
        }

        /// <summary>
        /// Create a new document
        /// </summary>
        [HttpPost("create_document")]
        public async Task<IActionResult> CreateDocument(CreateDocumentCommand command)
        {
            return Ok(await mediator.Send(command));
        }
    }
}
