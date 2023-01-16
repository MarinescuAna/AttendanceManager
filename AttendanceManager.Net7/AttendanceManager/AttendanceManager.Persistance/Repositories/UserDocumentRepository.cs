using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AttendanceManager.Persistance.Repositories
{
    public class UserDocumentRepository : GenericRepository<UserDocument>, IUserDocumentRepository
    {
        public UserDocumentRepository(AttendanceManagerDbContext dbContext) : base(dbContext)
        {
        }
        public override Task<UserDocument?> GetAsync(Expression<Func<UserDocument, bool>> expression, NavigationPropertiesSetting setting = NavigationPropertiesSetting.None)
            => setting switch
            {
                NavigationPropertiesSetting.OnlyReferenceNavigationProps => dbContext.UserDocuments.Include(s => s.Document).Include(s => s.User).FirstOrDefaultAsync(expression),
                _ => dbContext.UserDocuments.FirstOrDefaultAsync(expression)
            };

        public override Task<List<UserDocument>> ListAllAsync(NavigationPropertiesSetting setting = NavigationPropertiesSetting.None)
            => setting switch
            {
                NavigationPropertiesSetting.OnlyReferenceNavigationProps => dbContext.UserDocuments.Include(s => s.Document).Include(s => s.User).AsNoTracking().ToListAsync(),
                _ => dbContext.UserDocuments.AsNoTracking().ToListAsync(),
            };
    }
}
