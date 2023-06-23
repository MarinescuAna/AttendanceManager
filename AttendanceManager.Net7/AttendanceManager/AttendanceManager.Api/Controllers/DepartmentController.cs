using AttendanceManager.Application.Features.Department.Commands.CreateDepartment;
using AttendanceManager.Application.Features.Department.Commands.DeleteDepartment;
using AttendanceManager.Application.Features.Department.Commands.UpdateDepartmentName;
using AttendanceManager.Application.Features.Department.Queries.GetDepartments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManager.Api.Controllers
{
    /// <summary>
    /// NOTE: Don't delete departments because this means that the specializations should also be deleted.
    /// Also, don't delete specialization because there are users and courses and many more entities that depend on them and they also should be deleted!
    /// </summary>
    [Route("api/department"), ApiController, Authorize]
    public sealed class DepartmentController : BaseController
    {
        public DepartmentController(IMediator mediator, IHttpContextAccessor httpContextAccessor) : base(mediator, httpContextAccessor)
        {
        }

        /// <summary>
        /// Get all departments
        /// <returns>Success: list with all the departments without any filter</returns>
        /// </summary>
        [HttpGet("departments")]
        public async Task<IActionResult> GetDepartmentsAsync()
        {
            return Ok(await mediator.Send(new GetDepartmentQuery()));
        }

        /// <summary>
        /// Create a new department
        /// <returns>Success: the id of the new department added</returns>
        /// </summary>
        [HttpPost("create_department")]
        public async Task<IActionResult> CreateDepartmentAsync([FromBody] CreateDepatmentCommand command)
        {
            return Ok(await mediator.Send(command));
        }

        /// <summary>
        /// Update the department name
        /// </summary>
        /// <returns>Success: true/false</returns>
        [HttpPatch("update_department")]
        public async Task<IActionResult> UpdateDepartmentNameAsync([FromBody] UpdateDepartmentNameCommand command)
        {
            return Ok(await mediator.Send(command));
        }
        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> DeleteDepartmentAsync(int id)
        {
            return Ok(await mediator.Send(new DeleteDepartmentCommand() { DepartmentId = id }));
        }
    }
}

