using AttendanceManager.Application.Contracts.UnitOfWork;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Document.Queries.GetCreatedDocumentsByEmail
{
    public sealed class GetCreatedDocumentsByEmailQueryHandler : BaseFeature, IRequestHandler<GetCreatedDocumentsByEmailQuery, List<DocumentDto>>
    {
        public GetCreatedDocumentsByEmailQueryHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public async Task<List<DocumentDto>> Handle(GetCreatedDocumentsByEmailQuery request, CancellationToken cancellationToken)
        => mapper.Map<List<DocumentDto>>(
            await unitOfWork.DocumentRepository.GetUserDocumentsByExpressionAsync(u => u.Course!.UserSpecialization!.UserID == request.Email));

    }
}
