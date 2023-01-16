using MediatR;
using System.Text.Json.Serialization;

namespace AttendanceManager.Application.Features.Department.Commands.DeleteDepartment
{
    public sealed class DeleteDepartmentCommand : IRequest<bool>
    {
        [JsonPropertyName("id")]
        public required string DepartmentID { get; init; }
    }
}
