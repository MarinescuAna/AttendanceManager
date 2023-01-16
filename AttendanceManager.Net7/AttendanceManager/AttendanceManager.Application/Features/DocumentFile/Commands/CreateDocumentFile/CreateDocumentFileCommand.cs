﻿using MediatR;

namespace AttendanceManager.Application.Features.DocumentFile.Commands.CreateDocumentFile
{
    public sealed class CreateDocumentFileCommand : IRequest<bool>
    {
        public required string DocumentId { get; init; }
        public required string ActivityDateTime { get; init; }
        public required string CourseType { get; init; }
    }
}
