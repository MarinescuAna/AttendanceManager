using AttendanceManager.Application.Contracts.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Application.Shared;
using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Document.Queries.GetDocumentById
{
    public sealed class GetDocumentByIdQueryHandler : BaseFeature, IRequestHandler<GetDocumentByIdQuery, DocumentInfoDto>
    {
        private List<DocumentMember> _docMemberStudents = new();
        private Dictionary<int, CourseType> _collectionAttendances = new();
        public GetDocumentByIdQueryHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public async Task<DocumentInfoDto> Handle(GetDocumentByIdQuery request, CancellationToken cancellationToken)
        {
            //get current document
            var currentDocument = await unitOfWork.DocumentRepository.GetDocumentByIdAsync(request.Id)
                ?? throw new NotFoundException("Document", request.Id);
            await InitializeLists(request.Id);

            return new DocumentInfoDto
            {
                CourseId = currentDocument.CourseID,
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
                UpdateDate = currentDocument.UpdatedOn.ToString(Constants.DateFormat),
                NoLaboratories = _collectionAttendances.Count == 0 ? 0 : _collectionAttendances.Where(ca => ca.Value == CourseType.Laboratory).Count(),
                NoLessons = _collectionAttendances.Count == 0 ? 0 : _collectionAttendances.Where(ca => ca.Value == CourseType.Lesson).Count(),
                NoSeminaries = _collectionAttendances.Count == 0 ? 0 : _collectionAttendances.Where(ca => ca.Value == CourseType.Seminary).Count(),
                CreatedBy = currentDocument.Course!.UserSpecialization!.User!.FullName,
                AttendanceCollections = mapper.Map<AttendanceCollectionDto[]>(currentDocument.AttendanceCollections!.OrderByDescending(d => d.HeldOn)),
                DocumentMembers = mapper.Map<DocumentMembersDto[]>(_docMemberStudents),
                TotalAttendances = ComputeTotalAttendances(request.Role)
            };
        }

        private async Task InitializeLists(int documentId)
        {
            // get a dictionary of attendance collections: key: attendance collection Id, value: course type 
            _collectionAttendances = (await unitOfWork.AttendanceCollectionRepository.GetAttendanceCollectionsByDocumentIdAsync(documentId))
                .ToDictionary(ca => ca.AttendanceCollectionID, ca => ca.CourseType);

            // get all the students that are members of this document
            _docMemberStudents = await unitOfWork.DocumentMemberRepository.GetStudentsByDocumentIdAsync(documentId);
        }

        private TotalAttendanceDTO[] ComputeTotalAttendances(Role userRole)
        {
            TotalAttendanceDTO[] attendances = new TotalAttendanceDTO[_docMemberStudents.Count];

            // the user contains the attendances, but those are also from other document, so we will get only the attendances related to the collections from dictionary
            for (var i = 0; i < _docMemberStudents.Count; i++)
            {
                // for each student, get the attendances needed
                var userAttendances = _docMemberStudents[i].User!.Attendances!.Where(a => _collectionAttendances.ContainsKey(a.AttendanceCollectionID));

                attendances[i] = new()
                {
                    UserID = userRole == Role.Teacher ? _docMemberStudents[i].UserID : string.Empty,
                    UserName = userRole == Role.Teacher ? _docMemberStudents[i].User!.FullName : string.Empty,
                    Code = _docMemberStudents[i].User!.Code,
                    BonusPoints = userAttendances.Sum(a => a.BonusPoints),
                    CourseAttendances = userAttendances.Where(a => a.IsPresent && _collectionAttendances[a.AttendanceCollectionID] == CourseType.Lesson).Count(),
                    LaboratoryAttendances = userAttendances.Where(a => a.IsPresent && _collectionAttendances[a.AttendanceCollectionID] == CourseType.Laboratory).Count(),
                    SeminaryAttendances = userAttendances.Where(a => a.IsPresent && _collectionAttendances[a.AttendanceCollectionID] == CourseType.Seminary).Count(),
                };
            }

            return attendances;
        }
    }
}
