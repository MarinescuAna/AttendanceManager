using MediatR;
using System.Text.Json.Serialization;

namespace AttendanceManager.Application.Features.Department.Commands.UpdateDepartment
{
    public sealed class UpdateDepartmentCommand:IRequest<bool>
    {
        [JsonPropertyName("id")]
        public required string DepartmentID { get; init; }
        public required string Name { get; init; }

    }
}
