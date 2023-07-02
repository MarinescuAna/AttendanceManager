namespace AttendanceManagerGenerator.Modules
{
    public sealed class InvolvementsPostVm
    {
        public InvolvementDto[]? Involvements { get; set; }
        public string? CurrentUserEmail { get; set; }
        public string? CurrentUserName { get; set; }
    }

    public sealed class InvolvementDto
    {
        public required int InvolvementId { get; init; }
        public required int CollectionId { get; init; }
        public required int BonusPoints { get; init; }
        public required string UserId { get; init; }
        public required bool IsPresent { get; init; }
    }
}
