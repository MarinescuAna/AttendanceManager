using AttendanceManager.Application.Features.Department.Commands.CreateDepartment;
using AttendanceManager.Application.Features.Department.Commands.DeleteDepartment;
using AttendanceManager.Application.Features.Department.Commands.UpdateDepartment;
using AttendanceManager.Application.Features.Department.Queries.GetDepartments;
using AttendanceManager.Application.SharedDtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManager.Api.Controllers
{
    [Route("api/department")]
    [ApiController]
    //TODO [Authorize]
    public sealed class DepartmentController : BaseController
    {
        public DepartmentController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Get all departments 
        /// </summary>
        [HttpGet("departments")]
        public async Task<ActionResult<List<DepartmentDto>>> GetDepartments()
        {
            return Ok(await mediator.Send(new GetDepartmentQuery()));
        }

        /// <summary>
        /// Create a new department
        /// </summary>
        [HttpPost("create_department")]
        public async Task<ActionResult> CreateDepartment(string name)
        {
            return Ok(await mediator.Send(new CreateDepatmentCommand() { Name = name }));
        }

        /// <summary>
        /// Soft or hard delete an department
        /// </summary>
        /// <param name="name"></param>
        [HttpPatch("delete_department")]
        public async Task<ActionResult> DeleteDepartment(DeleteDepartmentCommand command)
        {
            return Ok(await mediator.Send(command));
        }

        /// <summary>
        /// Update the department name
        /// </summary>
        /// <param name="name"></param>
        [HttpPatch("update_department")]
        public async Task<ActionResult> UpdateDepartment(UpdateDepartmentCommand command)
        {
            return Ok(await mediator.Send(command));
        }
    }
}

