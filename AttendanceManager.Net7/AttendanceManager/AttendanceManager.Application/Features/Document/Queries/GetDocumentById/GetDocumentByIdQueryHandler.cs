﻿using AttendanceManager.Application.Contracts.UnitOfWork;
using AttendanceManager.Core.Shared;
using AttendanceManager.Application.Dtos;
using AttendanceManager.Domain.Enums;
using AutoMapper;
using MediatR;
using System.Globalization;

namespace AttendanceManager.Application.Features.Document.Queries.GetDocumentById
{
    public sealed class GetDocumentByIdQueryHandler : BaseDocumentFeature, IRequestHandler<GetDocumentByIdQuery, DocumentInfoDto>
    {
        public GetDocumentByIdQueryHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public async Task<DocumentInfoDto> Handle(GetDocumentByIdQuery request, CancellationToken cancellationToken)
        {
            //get the current document
            await this.InitialiazeDocumentAsync(request.Id);

            return new DocumentInfoDto
            {
                CourseId = currentDocument!.CourseID,
                CourseName = currentDocument.Course!.Name,
                CreationDate = currentDocument.CreatedOn.ToString(Constants.DateFormat),
                DocumentId = currentDocument.DocumentId,
                EnrollmentYear = currentDocument.EnrollmentYear,
                MaxNoLaboratories = currentDocument.MaxNoLaboratories,
                MaxNoLessons = currentDocument.MaxNoLessons,
                MaxNoSeminaries = currentDocument.MaxNoSeminaries,
                SpecializationId = currentDocument.Course!.UserSpecializationID,
                SpecializationName = currentDocument.Course!.UserSpecialization!.Specialization!.Name,
                Title = currentDocument.Title,
                UpdatedOn = currentDocument.UpdatedOn.ToString(Constants.DateFormat),
                NoLaboratories = attendanceCollectionsType!.Count == 0 ? 0 : attendanceCollectionsType.Where(ca => ca.Value == CourseType.Laboratory).Count(),
                NoLessons = attendanceCollectionsType.Count == 0 ? 0 : attendanceCollectionsType.Where(ca => ca.Value == CourseType.Lesson).Count(),
                NoSeminaries = attendanceCollectionsType.Count == 0 ? 0 : attendanceCollectionsType.Where(ca => ca.Value == CourseType.Seminary).Count(),
                CreatedBy = currentDocument.Course!.UserSpecialization!.User!.FullName,
                AttendanceCollections = mapper.Map<AttendanceCollectionDto[]>(currentDocument.AttendanceCollections!.OrderBy(d => d.HeldOn)),
                DocumentMembers = mapper.Map<DocumentMembersDto[]>(documentMembers),
                TotalAttendances = ComputeTotalAttendances(request.Role),
                AttendanceImportance = currentDocument.AttendanceImportance,
                BonusPointsImportance= currentDocument.BonusPointsImportance,
            };
        }

        private static TotalAttendanceDTO[] ComputeTotalAttendances(Role userRole)
        {
            var students = documentMembers!.Where(s => s!.User!.Role == Role.Student).ToList();
            TotalAttendanceDTO[] attendances = new TotalAttendanceDTO[students.Count];

            // the user contains the attendances, but those are also from other document, so we will get only the attendances related to the collections from dictionary
            for (var i = 0; i < students.Count; i++)
            {
                // for each student, get the attendances needed
                var userAttendances = students[i].User!.Attendances!.Where(a => attendanceCollectionsType!.ContainsKey(a.AttendanceCollectionID));

                attendances[i] = new()
                {
                    UserID = userRole == Role.Teacher ? students[i].UserID : string.Empty,
                    UserName = userRole == Role.Teacher ? students[i].User!.FullName : string.Empty,
                    Code = students[i].User!.Code,
                    BonusPoints = userAttendances.Sum(a => a.BonusPoints),
                    CourseAttendances = userAttendances.Where(a => a.IsPresent && attendanceCollectionsType![a.AttendanceCollectionID] == CourseType.Lesson).Count(),
                    LaboratoryAttendances = userAttendances.Where(a => a.IsPresent && attendanceCollectionsType![a.AttendanceCollectionID] == CourseType.Laboratory).Count(),
                    SeminaryAttendances = userAttendances.Where(a => a.IsPresent && attendanceCollectionsType![a.AttendanceCollectionID] == CourseType.Seminary).Count(),
                };
            }

            return attendances;
        }
    }
}
