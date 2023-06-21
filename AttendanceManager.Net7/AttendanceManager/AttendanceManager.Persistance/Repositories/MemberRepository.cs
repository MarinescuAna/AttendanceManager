using AttendanceManager.Application.Contracts.Infrastructure.Logging;
using AttendanceManager.Application.Contracts.Persistance.Repositories;
using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManager.Persistance.Repositories
{
    public class MemberRepository : GenericRepository<Member>, IMemberRepository
    {
        public MemberRepository(AttendanceManagerDbContext dbContext, ILoggingService loggingService) : base(dbContext, loggingService)
        {
        }

        /// <summary>
        /// This is a list with all the members of a given report. The role of the student can be passed as parameter, or can be null and in this case
        /// all the members of the report are returned
        /// Includes: User object and User Attendances object list
        /// </summary>
        public async Task<List<Member>> GetMembersByReportIdAndRoleAsync(int reportId, Role? role)
            => role == null ?
            await dbContext.Members
            .Include(u => u.User)
            .Include(dm => dm.User!.Involvements)
            .AsNoTracking()
            .Where(dm => dm.ReportID == reportId)
            .ToListAsync() :
        await dbContext.Members
            .Include(u => u.User)
            .Include(dm => dm.User!.Involvements)
            .AsNoTracking()
            .Where(dm => dm.User!.Role == role && dm.ReportID == reportId)
            .ToListAsync();

        public void DeleteMembersByReportId(int reportId)
        {
            var members = dbContext.Members.AsNoTracking().Where(dm => dm.ReportID == reportId);

            dbContext.RemoveRange(members);
        }
        public async Task AddRangeAsync(List<Member> entity)
        {
            try
            {
                await dbContext.Members.AddRangeAsync(entity);
            }
            catch (Exception ex)
            {
                loggingService.LogException(ex, System.Reflection.MethodBase.GetCurrentMethod()?.Name);
            }
        }
    }
}
