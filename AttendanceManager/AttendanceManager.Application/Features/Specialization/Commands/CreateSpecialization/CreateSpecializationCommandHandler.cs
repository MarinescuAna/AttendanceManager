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
            // Look for antoher specializations that have the same name and department and throw exception
            var specialization = await specializationRepository.GetAsync(s => s.Name == request.Name && s.DepartmentID==request.DepartmentId);

            if (specialization != null)
            {
                throw new AlreadyExistsException("Specialization", request.Name);
            }

            // Create the new specialization
            var newSpecialization = new Domain.Entities.Specialization
            {
                DepartmentID = request.DepartmentId,
                Name = request.Name,
                SpecializationID = Guid.NewGuid()
            };

            // Save the specialization or throw exception if something happen
            if(!await specializationRepository.AddAsync(newSpecialization))
            {
                throw new SomethingWentWrongException(Constants.SomethingWentWrongMessage);
            }

            // The specialziation id is mandatory later
            return mapper.Map<SpecializationDto>(newSpecialization);
        }
    }
}
