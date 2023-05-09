using AttendanceManager.Application.Contracts.Persistance.Repositories;
using AttendanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManager.Persistance.Repositories
{
    public class AttendanceCollectionRepository : GenericRepository<AttendanceCollection>, IAttendanceCollectionRepository
    {
        public AttendanceCollectionRepository(AttendanceManagerDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<AttendanceCollection>> GetAttendanceCollectionsByDocumentIdAsync(int documentId)
            => await dbContext.AttendanceCollections.AsNoTracking().Where(ac => ac.DocumentID == documentId).ToListAsync();

        public async Task<AttendanceCollection> GetAttendanceCollectionByIdAsync(int id)
            => await dbContext.AttendanceCollections
            .Include(ac => ac.Attendances!).AsNoTracking()
            .FirstOrDefaultAsync(a => a.AttendanceCollectionID == id);
    }
}
