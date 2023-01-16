using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AttendanceManager.Persistance.Repositories
{
    public class DocumentFileRepository : GenericRepository<DocumentFile>, IDocumentFileRepository
    {
        public DocumentFileRepository(AttendanceManagerDbContext dbContext) : base(dbContext)
        {
        }
        public override Task<DocumentFile?> GetAsync(Expression<Func<DocumentFile, bool>> expression, NavigationPropertiesSetting setting = NavigationPropertiesSetting.None)
            => setting switch
            {
                NavigationPropertiesSetting.None => dbContext.DocumentFiles.FirstOrDefaultAsync(expression),
                NavigationPropertiesSetting.OnlyReferenceNavigationProps => dbContext.DocumentFiles.Include(s => s.Document).FirstOrDefaultAsync(expression),
                NavigationPropertiesSetting.OnlyCollectionNavigationProps => dbContext.DocumentFiles.Include(s => s.Attendances).FirstOrDefaultAsync(expression),
                _ => dbContext.DocumentFiles.Include(s => s.Document).Include(s => s.Attendances).FirstOrDefaultAsync(expression)
            };

        public override Task<List<DocumentFile>> ListAllAsync(NavigationPropertiesSetting setting = NavigationPropertiesSetting.None)
            => setting switch
            {
                NavigationPropertiesSetting.None => dbContext.DocumentFiles.AsNoTracking().ToListAsync(),
                NavigationPropertiesSetting.OnlyReferenceNavigationProps => dbContext.DocumentFiles.Include(s => s.Document).ToListAsync(),
                NavigationPropertiesSetting.OnlyCollectionNavigationProps => dbContext.DocumentFiles.Include(s => s.Attendances).ToListAsync(),
                _ => dbContext.DocumentFiles.Include(s => s.Document).Include(s => s.Attendances).AsNoTracking().ToListAsync(),
            };
    }
}
