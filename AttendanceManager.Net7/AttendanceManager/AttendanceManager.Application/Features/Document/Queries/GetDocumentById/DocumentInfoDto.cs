﻿
using AttendanceManager.Application.Features.Attendance.Queries.GetStudentAttendanceByDocIdAndUserId;
using AttendanceManager.Application.Dtos;

namespace AttendanceManager.Application.Features.Document.Queries.GetDocumentById
{
    public sealed class DocumentInfoDto
    {
        public required int DocumentId { get; init; }
        public required string Title { get; init; }
        public required int EnrollmentYear { get; init; }
        public required int MaxNoSeminaries { get; init; }
        public required int MaxNoLaboratories { get; init; }
        public required int MaxNoLessons { get; init; }
        public required int NoSeminaries { get; init; }
        public required int NoLaboratories { get; init; }
        public required int NoLessons { get; init; }
        public required int CourseId { get; init; }
        public required string CourseName { get; init; }
        public required int SpecializationId { get; set; }
        public required string SpecializationName { get; set; }
        public required string CreationDate { get; init; }
        public required string UpdateDate { get; init; }
        public required int AttendanceImportance { get; init; }
        public required int BonusPointsImportance { get; init; }
        public required string CreatedBy { get; init; }
        public required AttendanceCollectionDto[] AttendanceCollections { get; init; }
        public required DocumentMembersDto[] DocumentMembers { get; init; }
        public StudentAttendancesDTO[]? CurrentStudentAttendances { get; set; }
        public required TotalAttendanceDTO[] TotalAttendances { get; init; }
    }

    public class AttendanceCollectionDto
    {
        public required int AttendanceCollectionId { get; init; }
        public required string ActivityTime { get; init; }
        public required string CourseType { get; init; }
    }

    public sealed class TotalAttendanceDTO
    {
        public required string UserID { get; init; }
        public required string UserName { get; init; }
        public required string Code { get; init; }
        public required int BonusPoints { get; init; }
        public required int CourseAttendances { get; init; }
        public required int LaboratoryAttendances { get; init; }
        public required int SeminaryAttendances { get; init; }
    }
}