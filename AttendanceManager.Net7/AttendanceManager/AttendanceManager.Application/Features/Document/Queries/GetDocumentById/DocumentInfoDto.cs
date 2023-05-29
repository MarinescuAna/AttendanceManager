﻿using AttendanceManager.Application.Dtos;

namespace AttendanceManager.Application.Features.Document.Queries.GetDocumentById
{
    public sealed class DocumentInfoDto
    {
        public required int DocumentId { get; init; }
        public required string Title { get; init; }
        public required int EnrollmentYear { get; init; }
        public required string CourseName { get; init; }
        public required string SpecializationName { get; set; }
        public required string UpdatedOn { get; set; }
        public required int MaxNoSeminaries { get; init; }
        public required int MaxNoLaboratories { get; init; }
        public required int MaxNoLessons { get; init; }
        public required int NoSeminaries { get; init; }
        public required int NoLaboratories { get; init; }
        public required int NoLessons { get; init; }
        public required int CourseId { get; init; }
        public required int SpecializationId { get; set; }
        public required string CreationDate { get; init; }
        public required int AttendanceImportance { get; init; }
        public required int BonusPointsImportance { get; init; }
        public required string CreatedBy { get; init; }
        public required bool IsCreator { get; init; }
        public required int NumberOfStudents { get; init; }
        public required AttendanceCollectionDto[] AttendanceCollections { get; init; }
        public required DocumentMembersDto[] DocumentMembers { get; init; }
    }

    public class AttendanceCollectionDto
    {
        public required int AttendanceCollectionId { get; init; }
        public required string ActivityTime { get; init; }
        public required string CourseType { get; init; }
    }
}