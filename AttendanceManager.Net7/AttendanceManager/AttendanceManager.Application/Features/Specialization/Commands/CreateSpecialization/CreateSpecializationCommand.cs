using MediatR;

namespace AttendanceManager.Application.Features.Specialization.Commands.CreateSpecialization
{
    public sealed class CreateSpecializationCommand : IRequest<Guid>
    {
        public required string Name { get; init; }
        public required Guid DepartmentId { get; init; }
    }
}
