using AttendanceManager.Application.Contracts.Infrastructure.Logging;
using AttendanceManager.Application.Contracts.Persistance.Repositories;
using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManager.Persistance.Repositories
{
    public class CollectionRepository : GenericRepository<Collection>, ICollectionRepository
    {
        public CollectionRepository(AttendanceManagerDbContext dbContext, ILoggingService loggingService) : base(dbContext,loggingService)
        {
        }
        /// <summary>
        /// Return all the collections related to a report by report id, INCLUDING the attendances that exists for each collection!
        /// Use this only if is needed!!
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public IQueryable<Collection> GetCollectionsByReportId(int reportId)
            => dbContext.Collections
                .AsNoTracking()
                .Include(ac => ac.Attendances)
                .Where(ac => ac.ReportID == reportId);
        public void DeleteCollectionsByReportId(int reportId)
        {
            var collections = dbContext.Collections.Where(c => c.ReportID == reportId).AsNoTracking();

            dbContext.Collections.RemoveRange(collections);
        }
        public void UpdateRange(List<Collection> entity)
        {
            try
            {
                dbContext.Set<Collection>().UpdateRange(entity);
            }
            catch (Exception ex)
            {
                loggingService.LogException(ex, System.Reflection.MethodBase.GetCurrentMethod()?.Name);
            }
        }
    }
}
