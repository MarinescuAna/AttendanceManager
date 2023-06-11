﻿using AttendanceManager.Application.Contracts.Infrastructure.Logging;
using AttendanceManager.Application.Contracts.Persistance.Repositories;
using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManager.Persistance.Repositories
{
    public class AttendanceCollectionRepository : GenericRepository<AttendanceCollection>, IAttendanceCollectionRepository
    {
        public AttendanceCollectionRepository(AttendanceManagerDbContext dbContext, ILoggingService loggingService) : base(dbContext,loggingService)
        {
        }
        /// <summary>
        /// Return all the collections related to a report by report id, INCLUDING the attendances that exists for each collection!
        /// Use this only if is needed!!
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public IQueryable<AttendanceCollection> GetCollectionsByReportId(int reportId)
            => dbContext.AttendanceCollections
                .AsNoTracking()
                .Include(ac => ac.Attendances)
                .Where(ac => ac.DocumentID == reportId);
        public void DeleteCollectionsByReportId(int reportId)
        {
            var collections = dbContext.AttendanceCollections.Where(c => c.DocumentID == reportId).AsNoTracking();

            dbContext.AttendanceCollections.RemoveRange(collections);
        }
        public void UpdateRange(List<AttendanceCollection> entity)
        {
            try
            {
                dbContext.Set<AttendanceCollection>().UpdateRange(entity);
            }
            catch (Exception ex)
            {
                loggingService.LogException(ex, System.Reflection.MethodBase.GetCurrentMethod()?.Name);
            }
        }
    }
}
