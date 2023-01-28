using MediatR;

namespace AttendanceManager.Application.Features.Department.Commands.DeleteDepartment
{
    /// <summary>
    /// Hard or soft delete, depending on the Specializations property
    /// </summary>
    public sealed class DeleteDepartmentCommand : IRequest<bool>
    {
        public required int DepartmentID { get; init; }
    }
}
