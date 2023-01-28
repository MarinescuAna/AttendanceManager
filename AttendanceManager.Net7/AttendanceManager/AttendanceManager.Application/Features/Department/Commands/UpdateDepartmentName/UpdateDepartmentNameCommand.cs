using MediatR;
using System.Text.Json.Serialization;

namespace AttendanceManager.Application.Features.Department.Commands.UpdateDepartmentName
{
    /// <summary>
    /// Update only the name
    /// </summary>
    public sealed class UpdateDepartmentNameCommand : IRequest<bool>
    {
        [JsonPropertyName("id")]
        public required int DepartmentID { get; init; }
        public required string Name { get; init; }

    }
}
