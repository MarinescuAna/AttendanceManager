using AttendanceManager.Application.Features.Course.Queries.GetCoursesQuery;
using AttendanceManager.Application.Features.DocumentFile.Commands.CreateDocumentFile;
using AttendanceManager.Application.Features.DocumentFile.Queries.GetDocumentFileByDocumentId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManager.Api.Controllers
{
    [Route("api/document_file")]
    [ApiController]
    public class DocumentFileController : BaseController
    {
        public DocumentFileController(IMediator mediator) : base(mediator)
        {
        }
        /// <summary>
        /// Create a new document file
        /// </summary>
        [HttpPost("create_document_file")]
        public async Task<IActionResult> CreateDocumentFile(CreateDocumentFileCommand command)
        {
            return Ok(await mediator.Send(command));
        }
        /// <summary>
        /// Get all courses
        /// </summary>
        /// <returns>Success: list with all documents files</returns>
        [HttpGet("document_files_by_documentId")]
        public async Task<IActionResult> GetDocumentFilesByDocumentId(string documentId)
        {
            return Ok(await mediator.Send(new GetDocumentFileByDocumentIdQuery() { DocumentId = documentId }));
        }

    }
}
