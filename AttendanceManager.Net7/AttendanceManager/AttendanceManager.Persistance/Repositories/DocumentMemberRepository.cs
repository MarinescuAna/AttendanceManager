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

        /// <summary>
        /// This is a list with all the STUDENTS members of a given document.
        /// Includes: User object and User Attendances object list
        /// </summary>
        /// <param name="documentId">the id of the document</param>
        /// <returns>List of document member class</returns>
        public async Task<List<DocumentMember>> GetStudentsByDocumentIdAsync(int documentId)
            => await dbContext.DocumentMembers
            .Include(u => u.User)
            .Include(dm=>dm.User!.Attendances)
            .AsNoTracking()
            .Where(dm => dm.User!.Role == Domain.Enums.Role.Student && dm.DocumentID == documentId)
            .ToListAsync();
    }
}
