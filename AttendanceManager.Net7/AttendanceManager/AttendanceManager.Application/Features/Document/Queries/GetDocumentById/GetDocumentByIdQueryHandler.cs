using AttendanceManager.Application.Contracts.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Document.Queries.GetDocumentById
{
    public sealed class GetDocumentByIdQueryHandler : BaseFeature, IRequestHandler<GetDocumentByIdQuery, DocumentInfoDto>
    {
        public GetDocumentByIdQueryHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public async Task<DocumentInfoDto> Handle(GetDocumentByIdQuery request, CancellationToken cancellationToken)
            => mapper.Map< DocumentInfoDto>(await unitOfWork.DocumentRepository.GetDocumentByIdAsync(request.Id)
                ?? throw new NotFoundException("Document",request.Id));
    }
}
