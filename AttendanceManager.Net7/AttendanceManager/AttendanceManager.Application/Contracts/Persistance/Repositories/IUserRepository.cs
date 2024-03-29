﻿using AttendanceManager.Domain.Entities;

namespace AttendanceManager.Application.Contracts.Persistance.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task AddRangeAsync(List<User> entities);
    }
}
