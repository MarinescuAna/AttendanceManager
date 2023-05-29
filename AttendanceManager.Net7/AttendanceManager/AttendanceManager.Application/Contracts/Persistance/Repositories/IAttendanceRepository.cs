﻿using AttendanceManager.Domain.Entities;

namespace AttendanceManager.Application.Contracts.Persistance.Repositories
{
    public interface IAttendanceRepository : IGenericRepository<Attendance>
    {
        IEnumerable<Attendance> GetAttendancesByReportId(int id);
    }
}
