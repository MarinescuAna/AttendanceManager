﻿using AttendanceManager.Domain.Enums;
using MediatR;

namespace AttendanceManager.Application.Features.Document.Queries.GetCreatedDocumentsByEmail
{
    public sealed class GetDocumentsQuery : IRequest<IEnumerable<DocumentDto>>
    {
        public required string Email { get; init; }
        public required Role Role { get; init; }
    }
}
