﻿using AttendanceManager.Domain.Entities;
using System.Linq.Expressions;

namespace AttendanceManager.Application.Contracts.Persistance
{
    public interface IDocumentRepository : IGenericRepository<Document>
    {
        Task<Document?> GetDocumentByIdAsync(int id);
        Task<List<Document>> GetUserDocumentsByExpressionAsync(Expression<Func<Document, bool>> expression);
    }
}
