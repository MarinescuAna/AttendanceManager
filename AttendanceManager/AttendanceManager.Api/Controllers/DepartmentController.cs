using AttendanceManager.Application.CommonVms;
using AttendanceManager.Application.Features.Department.Commands.CreateDepartment;
using AttendanceManager.Application.Features.Department.Queries.GetDepartments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AttendanceManager.Api.Controllers
{
    [Route("api/department")]
    [ApiController]
    //[Authorize]
    public class DepartmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DepartmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("departments")]
        public async Task<ActionResult<List<DepartmentVm>>> GetDepartments()
        {
            return Ok(await _mediator.Send(new GetDepartmentQuery()));
        }

        [HttpPost("create_department")]
        public async Task<ActionResult> CreateDepartment(string name) { 
            return Ok(await _mediator.Send(new CreateDepatmentCommand() { Name = name }));
        }
    }
}

