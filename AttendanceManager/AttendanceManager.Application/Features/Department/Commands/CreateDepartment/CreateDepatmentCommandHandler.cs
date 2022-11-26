using AttendanceManager.Application.CommonVms;
using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Application.Exceptions;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AttendanceManager.Application.Features.Department.Commands.CreateDepartment
{
    public class CreateDepatmentCommandHandler :DepartmentFeatureBase, IRequestHandler<CreateDepatmentCommand,DepartmentDto>
    {
        public CreateDepatmentCommandHandler(IDepartmentRepository departmentRepository, IMapper mapper) : base(departmentRepository, mapper)
        {
        }

        public async Task<DepartmentDto> Handle(CreateDepatmentCommand request, CancellationToken cancellationToken)
        {
            var department = await departmentRepository.GetAsync(d => d.Name == request.Name);

            if (department != null)
            {
                throw new AlreadyExistsException("Department", request.Name);
            }

            var newDepartment = new Domain.Entities.Department
            {
                DepartmentID = Guid.NewGuid(),
                Name = request.Name
            };

            await departmentRepository.AddAsync(newDepartment);

            return mapper.Map<DepartmentDto>(newDepartment);
        }
    }
}
