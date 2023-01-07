using AttendanceManager.Application.Features.Department.Commands.CreateDepartment;
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
        /// Get all departments together with each specialization => departments + specializations = organizations
        /// </summary>
        /// <returns>Success: list with all departments and specializations</returns>
        [HttpGet("departments")]
        public async Task<ActionResult<List<OrganizationDto>>> GetDepartments()
        {
            return Ok(await mediator.Send(new GetDepartmentQuery()));
        }

        /// <summary>
        /// Create only a new department, no link to specializations
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Success: return the name and the id of the new department</returns>
        [HttpPost("create_department")]
        public async Task<ActionResult> CreateDepartment(string name)
        {
            return Ok(await mediator.Send(new CreateDepatmentCommand() { Name = name }));
        }

        /// <summary>
        /// Soft delete an department
        /// </summary>
        /// <param name="name"></param>
        [HttpPatch("delete_department")]
        public async Task<ActionResult> DeleteDepartment(UpdateOrDeleteDepartmentCommand command)
        {
            return Ok(await mediator.Send(command));
        }

        /// <summary>
        /// Update the department name
        /// </summary>
        /// <param name="name"></param>
        [HttpPatch("update_department")]
        public async Task<ActionResult> UpdateDepartment(UpdateOrDeleteDepartmentCommand command)
        {
            return Ok(await mediator.Send(command));
        }
    }
}

