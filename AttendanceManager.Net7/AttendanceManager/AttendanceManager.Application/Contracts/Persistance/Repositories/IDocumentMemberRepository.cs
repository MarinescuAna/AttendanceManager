﻿using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;

namespace AttendanceManager.Application.Contracts.Persistance.Repositories
{
    public interface IDocumentMemberRepository : IGenericRepository<DocumentMember>
    {
        Task<List<DocumentMember>> GetDocumentMembersByDocumentIdAndRoleAsync(int documentId, Role? role);
        Task<DocumentMember?> GetMemberByDocumentIdAndUserIdAsync(int documentId, string userId);
        void DeleteMembersByDocumentId(int documentId);
    }
}