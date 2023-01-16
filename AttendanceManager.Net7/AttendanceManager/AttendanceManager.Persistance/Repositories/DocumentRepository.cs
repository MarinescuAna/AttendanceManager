using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AttendanceManager.Persistance.Repositories
{
    public class DocumentRepository : GenericRepository<Document>, IDocumentRepository
    {
        public DocumentRepository(AttendanceManagerDbContext dbContext) : base(dbContext)
        {
        }

        public override Task<Document?> GetAsync(Expression<Func<Document, bool>> expression, NavigationPropertiesSetting setting = NavigationPropertiesSetting.None)
            => setting switch
            {
                NavigationPropertiesSetting.None => dbContext.Documents.FirstOrDefaultAsync(expression),
                NavigationPropertiesSetting.OnlyReferenceNavigationProps => dbContext.Documents.Include(s => s.Course).Include(s => s.Specialization).Include(s => s.User).FirstOrDefaultAsync(expression),
                NavigationPropertiesSetting.OnlyCollectionNavigationProps => dbContext.Documents.Include(s => s.UserDocuments).FirstOrDefaultAsync(expression),
                _ => dbContext.Documents.Include(s => s.Specialization).Include(s => s.Course).Include(s => s.User).Include(s => s.UserDocuments).FirstOrDefaultAsync(expression)
            };

        public override Task<List<Document>> ListAllAsync(NavigationPropertiesSetting setting = NavigationPropertiesSetting.None)
            => setting switch
            {
                NavigationPropertiesSetting.None => dbContext.Documents.AsNoTracking().Where(u => !u.IsDeleted).ToListAsync(),
                NavigationPropertiesSetting.OnlyReferenceNavigationProps => dbContext.Documents.Include(s => s.Course).Include(s => s.Specialization).Include(s => s.User).ToListAsync(),
                NavigationPropertiesSetting.OnlyCollectionNavigationProps => dbContext.Documents.Include(s => s.UserDocuments).ToListAsync(),
                _ => dbContext.Documents.Include(s => s.Specialization).Include(s => s.Course).Include(s => s.User).Include(s => s.UserDocuments).AsNoTracking().Where(u => !u.IsDeleted).ToListAsync(),
            };
    }
}
