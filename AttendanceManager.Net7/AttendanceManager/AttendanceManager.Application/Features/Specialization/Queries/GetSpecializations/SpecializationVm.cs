﻿namespace AttendanceManager.Application.Features.Specialization.Queries.GetSpecializations
{
    public sealed class SpecializationVm
    {
        public required int Id { get; init; }
        public required string Name { get; init; }
        public required int DepartmentId { get; init; }
        public required int UsersLinked { get; init; }
        public required string UpdatedOn { get; init; }

    }
}
