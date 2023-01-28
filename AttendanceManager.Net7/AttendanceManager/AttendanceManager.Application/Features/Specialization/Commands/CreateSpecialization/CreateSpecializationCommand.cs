using MediatR;

namespace AttendanceManager.Application.Features.Specialization.Commands.CreateSpecialization
{
    public sealed class CreateSpecializationCommand : IRequest<int>
    {
        public required string Name { get; init; }
        public required int DepartmentId { get; init; }
    }
}
