using AttendanceManager.Application.Contracts.Infrastructure.Logging;
using AttendanceManager.Application.Contracts.Persistance.Repositories;
using AttendanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManager.Persistance.Repositories
{
    public class AttendanceCollectionRepository : GenericRepository<AttendanceCollection>, IAttendanceCollectionRepository
    {
        public AttendanceCollectionRepository(AttendanceManagerDbContext dbContext, ILoggingService loggingService) : base(dbContext,loggingService)
        {
        }

        public async Task<List<AttendanceCollection>> GetAttendanceCollectionsByDocumentIdAsync(int documentId)
            => await dbContext.AttendanceCollections.AsNoTracking().Where(ac => ac.DocumentID == documentId).ToListAsync();
        public bool HasAttendanceByReportIdUserId(int reportId, string email)
            => dbContext.AttendanceCollections.AsNoTracking()
            .Include(ac => ac.Attendances)
            .Where(ac => ac.DocumentID == reportId)
            .Where(ac => ac.Attendances!=null && ac.Attendances!.Count() != 0)
            .Any(ac => ac.Attendances != null && ac.Attendances!.Any(a => a.UserID == email && a.IsPresent));
        public async Task<AttendanceCollection> GetAttendanceCollectionByIdAsync(int id)
            => await dbContext.AttendanceCollections
            .Include(ac => ac.Attendances).AsNoTracking()
            .FirstOrDefaultAsync(a => a.AttendanceCollectionID == id);
    }
}
