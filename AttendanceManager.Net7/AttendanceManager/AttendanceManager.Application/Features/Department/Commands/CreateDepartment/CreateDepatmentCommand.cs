using MediatR;

namespace AttendanceManager.Application.Features.Department.Commands.CreateDepartment
{
    /// <summary>
    /// Create a new department. This can be done only by the admin
    /// </summary>
    public sealed class CreateDepatmentCommand : IRequest<int>
    {
        public required string Name { get; init; }
    }
}
