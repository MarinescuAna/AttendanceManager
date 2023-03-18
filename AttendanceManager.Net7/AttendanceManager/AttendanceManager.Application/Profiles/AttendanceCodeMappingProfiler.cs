using AttendanceManager.Application.Features.AttendanceCode.Commands.CreateAttendanceCode;
using AttendanceManager.Domain.Entities;
using AutoMapper;

namespace AttendanceManager.Application.Profiles
{
    public sealed class AttendanceCodeMappingProfiler : Profile
    {
        public AttendanceCodeMappingProfiler()
        {
            CreateMap<AttendanceCode, AttendanceCodeDTO>();
        }
    }
}
