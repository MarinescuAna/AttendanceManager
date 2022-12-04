using AttendanceManager.Application.Contracts.Persistance;
using AutoMapper;

namespace AttendanceManager.Application.Features.Specialization
{
    public abstract class SpecializationFeatureBase
    {
        protected readonly ISpecializationRepository specializationRepository;
        protected readonly IMapper mapper;

        protected SpecializationFeatureBase(ISpecializationRepository specializationRepository, IMapper mapper)
        {
            this.specializationRepository = specializationRepository;
            this.mapper = mapper;
        }
    }
}
