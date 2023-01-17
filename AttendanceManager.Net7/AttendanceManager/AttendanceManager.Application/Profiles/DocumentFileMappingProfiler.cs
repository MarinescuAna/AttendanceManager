using AttendanceManager.Application.Features.DocumentFile.Queries.GetDocumentFileByDocumentId;
using AttendanceManager.Domain.Entities;
using AutoMapper;

namespace AttendanceManager.Application.Profiles
{
    public sealed class DocumentFileMappingProfiler : Profile
    {
        public DocumentFileMappingProfiler()
        {
            CreateMap<DocumentFile, DocumentFileDto>();
        }
    }
}
