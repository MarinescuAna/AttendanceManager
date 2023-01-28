using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Domain.Entities;

namespace AttendanceManager.Persistance.Repositories
{
    public class DocumentMemberRepository : GenericRepository<DocumentMember>, IDocumentMemberRepository
    {
        public DocumentMemberRepository(AttendanceManagerDbContext dbContext) : base(dbContext)
        {
        }

    }
}
