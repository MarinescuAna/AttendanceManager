using AttendanceManagerGenerator.Utils;

namespace AttendanceManagerGenerator.Modules
{
    public sealed class InvolvementGetResponseVm
    {
        public required int InvolvementId { get; init; }
        public required int CollectionId { get; init; }
        public required int BonusPoints { get; init; }
        public required ActivityType ActivityType { get; init; }
        public required string Student { get; init; }
        public required string Email { get; init; }
        public string? Title { get; init; }
        public string? HeldOn { get; init; }
        public bool IsPresent { get; init; }
        public required string UpdateOn { get; init; }
        public required string UpdateBy { get; init; }
    }
}
