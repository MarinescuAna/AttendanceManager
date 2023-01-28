using MediatR;

namespace AttendanceManager.Application.Features.AttendanceCollection.Commands.CreateAttendanceCollection
{
    public sealed class CreateAttendanceCollectionCommand : IRequest<int>
    {
        public required int DocumentId { get; init; }
        public required string ActivityDateTime { get; init; }
        public required string CourseType { get; init; }
    }
}
