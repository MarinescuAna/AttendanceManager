using AttendanceManager.Application.Contracts.Infrastructure.Logging;
using AttendanceManager.Application.Contracts.Persistance.Repositories;
using AttendanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AttendanceManager.Persistance.Repositories
{
    public class DocumentRepository : GenericRepository<Document>, IDocumentRepository
    {
        public DocumentRepository(AttendanceManagerDbContext dbContext, ILoggingService loggingService) : base(dbContext,loggingService)
        {
        }

        /// <summary>
        /// The result will contain the Specialization, Course and UserSpecialization
        /// </summary>
        public async Task<Document?> GetDocumentByIdAsync(int id)
            => await dbContext.Documents
            .Include(d=>d.Collections)
            .Include(d=>d.Course!.UserSpecialization!.Specialization)
            .Include(d=>d.Course!.UserSpecialization!.User)
            .FirstOrDefaultAsync(u => u.DocumentId == id);

        /// <summary>
        /// The list will contain the Specialization, Course and UserSpecialization
        /// </summary>
        public async Task<List<Document>> GetUserDocumentsByExpressionAsync(Expression<Func<Document, bool>> expression)
            => await dbContext.Documents
            .Include(d => d.Course!.UserSpecialization!.Specialization)
            .Include(m=>m.Members)
            .AsNoTracking().Where(expression).ToListAsync();
    }
}
