using AttendanceManager.Application.Features.DocumentFile.Commands.CreateDocumentFile;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManager.Api.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<ActionResult> CreateDocumentFile(CreateDocumentFileCommand command)
        {
            return Ok(await mediator.Send(command));
        }

    }
}
