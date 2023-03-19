namespace AttendanceManager.Application.Features.Document.Queries.GetDocumentById
{
    public sealed class DocumentMembersDto
    {
        public required string Email { get; init; }
        public required string Name { get; init; }
        public required string Role { get; init; }
    }
}
