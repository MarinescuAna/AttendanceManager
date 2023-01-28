using AttendanceManager.Application.Features.Department.Commands.CreateDepartment;
using AttendanceManager.Application.Features.Department.Commands.DeleteDepartment;
using AttendanceManager.Application.Features.Department.Commands.UpdateDepartmentName;
using AttendanceManager.Application.Features.Department.Queries.GetDepartments;
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
        /// <returns>Success: list with all the departments without any filter</returns>
        /// </summary>
        [HttpGet("departments")]
        public async Task<IActionResult> GetDepartments()
        {
            return Ok(await mediator.Send(new GetDepartmentQuery()));
        }

        /// <summary>
        /// Create a new department
        /// <returns>Success: the id of the new department added</returns>
        /// </summary>
        [HttpPost("create_department")]
        public async Task<IActionResult> CreateDepartment(string name)
        {
            return Ok(await mediator.Send(new CreateDepatmentCommand() { Name = name }));
        }

        /// <summary>
        /// Soft or hard delete an department
        /// </summary>
        /// <returns>Success: true/false</returns>
        [HttpPatch("delete_department")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            return Ok(await mediator.Send(new DeleteDepartmentCommand { DepartmentID = id }));
        }

        /// <summary>
        /// Update the department name
        /// </summary>
        /// <returns>Success: true/false</returns>
        [HttpPatch("update_department_name")]
        public async Task<IActionResult> UpdateDepartmentName([FromBody] UpdateDepartmentNameCommand command)
        {
            return Ok(await mediator.Send(command));
        }
    }
}

