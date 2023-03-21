using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;
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
        /// This is a list with all the members of a given document. The role of the student can be passed as parameter, or can be null and in this case
        /// all the members of the document are returned
        /// Includes: User object and User Attendances object list
        /// </summary>
        /// <param name="documentId">the id of the document</param>
        /// <returns>List of document member class</returns>
        public async Task<List<DocumentMember>> GetDocumentMembersByDocumentIdAndRoleAsync(int documentId, Role? role)
            => role == null ?
            await dbContext.DocumentMembers
            .Include(u => u.User)
            .Include(dm => dm.User!.Attendances)
            .AsNoTracking()
            .Where(dm => dm.DocumentID == documentId)
            .ToListAsync() :
        await dbContext.DocumentMembers
            .Include(u => u.User)
            .Include(dm => dm.User!.Attendances)
            .AsNoTracking()
            .Where(dm => dm.User!.Role == role && dm.DocumentID == documentId)
            .ToListAsync();
    }
}
