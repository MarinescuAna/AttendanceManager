﻿using AttendanceManager.Domain.Entities;
using System.Linq.Expressions;

namespace AttendanceManager.Application.Contracts.Persistance.Repositories
{
    public interface IAttendanceRepository : IGenericRepository<Attendance>
    {
        Task<bool> HasAttendanceAsync(Expression<Func<Attendance, bool>> expression);
    }
}