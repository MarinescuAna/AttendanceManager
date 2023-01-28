using AttendanceManager.Application.Contracts.UnitOfWork;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.AttendanceCollection.Queries.GetAttendanceCollectionByDocumentId
{
    public sealed class GetAttendanceCollectionByDocumentIdQueryHandler : BaseFeature, IRequestHandler<GetAttendanceCollectionByDocumentIdQuery, List<AttendanceCollectionDto>>
    {
        public GetAttendanceCollectionByDocumentIdQueryHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public async Task<List<AttendanceCollectionDto>> Handle(GetAttendanceCollectionByDocumentIdQuery request, CancellationToken cancellationToken)
          => mapper.Map<List<AttendanceCollectionDto>>((await unitOfWork.AttendanceCollectionRepository.ListAllAsync())
              .Where(df => df.DocumentID == request.DocumentId)); 
    }
}
