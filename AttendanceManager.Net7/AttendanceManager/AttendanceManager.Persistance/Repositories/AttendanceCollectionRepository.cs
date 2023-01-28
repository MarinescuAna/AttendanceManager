using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AttendanceManager.Persistance.Repositories
{
    public class AttendanceCollectionRepository : GenericRepository<AttendanceCollection>, IAttendanceCollectionRepository
    {
        public AttendanceCollectionRepository(AttendanceManagerDbContext dbContext) : base(dbContext)
        {
        }
    }
}
