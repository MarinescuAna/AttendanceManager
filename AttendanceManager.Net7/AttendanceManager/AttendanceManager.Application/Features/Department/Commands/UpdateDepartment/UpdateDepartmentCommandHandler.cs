using AttendanceManager.Application.Contracts.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Department.Commands.UpdateDepartment
{
    public sealed class UpdateDepartmentCommandHandler : BaseFeature, IRequestHandler<UpdateDepartmentCommand, bool>
    {
        public UpdateDepartmentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<bool> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
        {
            //get the department
            var department = await unitOfWork.DepartmentRepository.GetAsync(u => u.DepartmentID.ToString() == request.DepartmentID && !u.IsDeleted)
                ?? throw new NotFoundException("Department", request.DepartmentID);

            // check if the name is unique
            if (await unitOfWork.DepartmentRepository.GetAsync(u => u.Name == request.Name && !u.IsDeleted) != null)
            {
                throw new AlreadyExistsException("Department", request.Name);
            }

            department.Name = request.Name;
            unitOfWork.DepartmentRepository.Update(department);

            return await unitOfWork.CommitAsync();
        }
    }
}
