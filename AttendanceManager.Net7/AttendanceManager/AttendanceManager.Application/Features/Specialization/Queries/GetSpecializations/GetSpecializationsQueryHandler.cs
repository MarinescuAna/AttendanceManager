using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Specialization.Queries.GetSpecializations
{
    public sealed class GetSpecializationsQueryHandler : BaseFeature, IRequestHandler<GetSpecializationsQuery, List<SpecializationDto>>
    {
        public GetSpecializationsQueryHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public async Task<List<SpecializationDto>> Handle(GetSpecializationsQuery request, CancellationToken cancellationToken)
            => mapper.Map<List<SpecializationDto>>(await unitOfWork.SpecializationRepository.ListAllAsync());
    }
}
