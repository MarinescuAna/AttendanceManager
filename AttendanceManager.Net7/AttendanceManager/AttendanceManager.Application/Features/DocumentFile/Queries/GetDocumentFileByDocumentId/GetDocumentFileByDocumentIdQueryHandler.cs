using AttendanceManager.Application.Contracts.UnitOfWork;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.DocumentFile.Queries.GetDocumentFileByDocumentId
{
    public sealed class GetDocumentFileByDocumentIdQueryHandler : BaseFeature, IRequestHandler<GetDocumentFileByDocumentIdQuery, List<DocumentFileDto>>
    {
        public GetDocumentFileByDocumentIdQueryHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public async Task<List<DocumentFileDto>> Handle(GetDocumentFileByDocumentIdQuery request, CancellationToken cancellationToken)
          => mapper.Map<List<DocumentFileDto>>((await unitOfWork.DocumentFileRepository.ListAllAsync())
              .Where(df => df.DocumentID.ToString() == request.DocumentId)); 
    }
}
