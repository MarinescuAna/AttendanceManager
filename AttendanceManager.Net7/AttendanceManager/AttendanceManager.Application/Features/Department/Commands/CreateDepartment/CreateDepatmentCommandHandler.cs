using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Application.Shared;
using AttendanceManager.Application.SharedDtos;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Department.Commands.CreateDepartment
{
    public sealed class CreateDepatmentCommandHandler : DepartmentFeatureBase, IRequestHandler<CreateDepatmentCommand, DepartmentDto>
    {
        public CreateDepatmentCommandHandler(IDepartmentRepository departmentRepository, IMapper mapper) : base(departmentRepository, mapper)
        {
        }

        public async Task<DepartmentDto> Handle(CreateDepatmentCommand request, CancellationToken cancellationToken)
        {
            // Look for other departments with the same name and throw exception if exists
            if (await departmentRepository.GetAsync(d => d.Name == request.Name, false) != null)
            {
                throw new AlreadyExistsException("Department", request.Name);
            }

            // Create the new department
            var newDepartment = new Domain.Entities.Department
            {
                DepartmentID = Guid.NewGuid(),
                Name = request.Name
            };

            // Add new department or throw exception if something happen
            if (!await departmentRepository.AddAsync(newDepartment))
            {
                throw new SomethingWentWrongException(Constants.SomethingWentWrongMessage);
            }

            // Return the created department [the department id is mandatory]
            return mapper.Map<DepartmentDto>(newDepartment);
        }
    }
}
