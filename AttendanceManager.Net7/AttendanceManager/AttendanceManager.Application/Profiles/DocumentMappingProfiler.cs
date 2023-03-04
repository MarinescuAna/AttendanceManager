using AttendanceManager.Application.Features.Document.Queries.GetCreatedDocumentsByEmail;
using AttendanceManager.Application.Features.Document.Queries.GetDocumentById;
using AttendanceManager.Domain.Entities;
using AutoMapper;

namespace AttendanceManager.Application.Profiles
{
    public sealed class DocumentMappingProfiler : Profile
    {
        public DocumentMappingProfiler()
        {
            CreateMap<Document, DocumentDto>()
                .ForMember(d => d.CourseName, act => act.MapFrom(d => d.Course!.Name))
                .ForMember(d => d.SpecializationName, act => act.MapFrom(d => d.Course!.UserSpecialization!.Specialization!.Name));
        }
    }
}
