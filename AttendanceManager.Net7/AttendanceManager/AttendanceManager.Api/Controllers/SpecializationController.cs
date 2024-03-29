﻿using AttendanceManager.Application.Features.Specialization.Commands.CreateSpecialization;
using AttendanceManager.Application.Features.Specialization.Commands.DeleteSpecialization;
using AttendanceManager.Application.Features.Specialization.Queries.GetSpecializations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManager.Api.Controllers
{
    /// <summary>
    /// NOTE: Don't delete specialization because there are users and courses and many more entities that depend on them and they also should be deleted!
    /// </summary>
    [Route("api/specialization"), ApiController, Authorize]
    public sealed class SpecializationController : BaseController
    {
        public SpecializationController(IMediator mediator, IHttpContextAccessor httpContextAccessor) : base(mediator, httpContextAccessor)
        {
        }

        /// <summary>
        /// Get all specializations 
        /// <returns>Success: list with all the specializations without any filter</returns>
        /// </summary>
        [HttpGet("specializations")]
        public async Task<IActionResult> GetSpecializationsAsync()
        {
            return Ok(await mediator.Send(new GetSpecializationsQuery()));
        }

        /// <summary>
        /// Create a specialization
        /// <returns>Success: true/false</returns>
        /// </summary>
        [HttpPost("create_specialization")]
        public async Task<IActionResult> CreateSpecializationAsync([FromBody] CreateSpecializationCommand command)
        {
            return Ok(await mediator.Send(command));
        }

        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> DeleteSpecializationAsync(int id)
        {
            return Ok(await mediator.Send(new DeleteSpecializationCommand() { SpecializationId = id }));
        }
    }
}
