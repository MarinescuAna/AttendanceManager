using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManager.Persistance.Repositories
{
    public class DocumentMemberRepository : GenericRepository<DocumentMember>, IDocumentMemberRepository
    {
        public DocumentMemberRepository(AttendanceManagerDbContext dbContext) : base(dbContext)
        {
        }
        public async Task<DocumentMember?> GetMemberByDocumentIdAndUserIdAsync(int documentId, string userId)
            => await dbContext.DocumentMembers
            .Include(dm => dm.User!.Attendances)
            .Include(dm => dm.Document!.AttendanceCollections)
            .AsNoTracking()
            .FirstOrDefaultAsync(dm => dm.UserID == userId && dm.DocumentID == documentId);
        public async Task<List<User>> GetStudentsByDocumentIdAsync(int documentId)
            => await dbContext.DocumentMembers
            .Include(u => u.User)
            .Include(dm=>dm.User!.Attendances)
            .AsNoTracking()
            .Where(dm => dm.User!.Role == Domain.Enums.Role.Student && dm.DocumentID == documentId)
            .Select(dm => dm.User!)
            .ToListAsync();
    }
}
