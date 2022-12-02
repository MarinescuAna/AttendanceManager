using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Application.Shared;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AttendanceManager.Application.Features.Specialization.Commands.CreateSpecialization
{
    public class CreateSpecializationCommandHandler : SpecializationFeatureBase, IRequestHandler<CreateSpecializationCommand,SpecializationDto>
    {
        public CreateSpecializationCommandHandler(ISpecializationRepository specializationRepository, IMapper mapper) : base(specializationRepository, mapper)
        {
        }

        public async Task<SpecializationDto> Handle(CreateSpecializationCommand request, CancellationToken cancellationToken)
        {
            var specialization = await specializationRepository.GetAsync(s => s.Name == request.Name && s.DepartmentID==request.DepartmentId);

            if (specialization != null)
            {
                throw new AlreadyExistsException("Specialization", request.Name);
            }

            var newSpecialization = new Domain.Entities.Specialization
            {
                DepartmentID = request.DepartmentId,
                Name = request.Name,
                SpecializationID = Guid.NewGuid()
            };

            if(!await specializationRepository.AddAsync(newSpecialization))
            {
                throw new SomethingWentWrongException(Constants.SomethingWentWrongMessage);
            }

            return mapper.Map<SpecializationDto>(newSpecialization);
        }
    }
}
