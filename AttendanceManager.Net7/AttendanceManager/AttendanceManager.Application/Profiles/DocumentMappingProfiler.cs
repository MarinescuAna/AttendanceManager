using AttendanceManager.Application.Dtos;
using AttendanceManager.Application.Features.Document.Queries.GetDocumentById;
using AttendanceManager.Domain.Common;
using AttendanceManager.Domain.Entities;
using AutoMapper;

namespace AttendanceManager.Application.Profiles
{
    public sealed class DocumentMappingProfiler : Profile
    {
        public DocumentMappingProfiler()
        {
            CreateMap<Document, DocumentBaseDto>()
                .ForMember(d => d.CourseName, act => act.MapFrom(d => d.Course!.Name))
                .ForMember(d => d.UpdatedOn, act => act.MapFrom(d => d.UpdatedOn.ToString(Constants.ShortDateFormat)))
                .ForMember(d => d.SpecializationName, act => act.MapFrom(d => d.Course!.UserSpecialization!.Specialization!.Name));
            CreateMap<AttendanceCollection, AttendanceCollectionDto>()
                .ForMember(a => a.ActivityTime, act => act.MapFrom(ac => ac.HeldOn.ToString(Constants.ShortDateFormat)));
            CreateMap<DocumentMember, DocumentMembersDto>()
                .ForMember(u => u.Name, act => act.MapFrom(d => d.User!.FullName))
                .ForMember(u => u.Email, act => act.MapFrom(d => d.User!.Email));
        }
    }
}
