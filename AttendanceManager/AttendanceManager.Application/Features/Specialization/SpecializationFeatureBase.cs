using AttendanceManager.Application.Contracts.Persistance;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

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
