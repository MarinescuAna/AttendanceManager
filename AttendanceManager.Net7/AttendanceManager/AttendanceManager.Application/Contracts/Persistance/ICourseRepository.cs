﻿using AttendanceManager.Domain.Entities;

namespace AttendanceManager.Application.Contracts.Persistance
{
    public interface ICourseRepository : IGenericRepository<Course>
    {
        Task<bool> SoftOrHardDelete(int courseId);
        Task<List<Course>> GetTeacherCoursesByEmailAsync(string email);
    }
}
