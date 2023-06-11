﻿using AttendanceManager.Application.Contracts.Infrastructure.Logging;
using AttendanceManager.Application.Contracts.Persistance.Repositories;
using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManager.Persistance.Repositories
{
    public class DocumentMemberRepository : GenericRepository<DocumentMember>, IDocumentMemberRepository
    {
        public DocumentMemberRepository(AttendanceManagerDbContext dbContext, ILoggingService loggingService) : base(dbContext, loggingService)
        {
        }

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

        public void DeleteMembersByDocumentId(int documentId)
        {
            var members = dbContext.DocumentMembers.AsNoTracking().Where(dm => dm.DocumentID == documentId);

            dbContext.RemoveRange(members);
        }
        public async Task AddRangeAsync(List<DocumentMember> entity)
        {
            try
            {
                await dbContext.DocumentMembers.AddRangeAsync(entity);
            }
            catch (Exception ex)
            {
                loggingService.LogException(ex, System.Reflection.MethodBase.GetCurrentMethod()?.Name);
            }
        }
    }
}
