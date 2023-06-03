using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Specialization.Queries.GetSpecializations
{
    public sealed class GetSpecializationsQuery : IRequest<SpecializationVm[]>
    {
        public sealed class GetSpecializationsQueryHandler : IRequestHandler<GetSpecializationsQuery, SpecializationVm[]>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public GetSpecializationsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public Task<SpecializationVm[]> Handle(GetSpecializationsQuery request, CancellationToken cancellationToken)
                => Task.FromResult(_mapper.Map<SpecializationVm[]>(_unitOfWork.SpecializationRepository.ListAll().ToArray()));
        }
    }
}
