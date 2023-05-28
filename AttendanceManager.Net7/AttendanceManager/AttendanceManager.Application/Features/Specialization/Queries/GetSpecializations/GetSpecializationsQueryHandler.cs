using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Specialization.Queries.GetSpecializations
{
    public sealed class GetSpecializationsQueryHandler : BaseFeature, IRequestHandler<GetSpecializationsQuery, SpecializationDto[]>
    {
        public GetSpecializationsQueryHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public Task<SpecializationDto[]> Handle(GetSpecializationsQuery request, CancellationToken cancellationToken)
            => Task.FromResult(mapper.Map<SpecializationDto[]>(unitOfWork.SpecializationRepository.ListAll().ToArray()));
    }
}
