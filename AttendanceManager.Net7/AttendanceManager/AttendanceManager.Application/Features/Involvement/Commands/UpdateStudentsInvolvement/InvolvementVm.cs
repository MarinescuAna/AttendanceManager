﻿namespace AttendanceManager.Application.Features.Involvement.Commands.UpdateStudentsInvolvement
{
    public sealed class InvolvementVm
    {
        public required int InvolvementId { get; init; }
        public required int CollectionId { get; init; }
        public required int BonusPoints { get; init; }
        public required string UserId { get; init; }
        public required bool IsPresent { get; init; }
    }
}
