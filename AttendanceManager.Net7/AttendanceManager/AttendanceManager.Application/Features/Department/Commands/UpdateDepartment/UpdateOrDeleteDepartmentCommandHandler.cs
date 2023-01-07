using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Application.Exceptions;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Department.Commands.UpdateDepartment
{
    public sealed class UpdateOrDeleteDepartmentCommandHandler : DepartmentFeatureBase, IRequestHandler<UpdateOrDeleteDepartmentCommand, bool>
    {
        public UpdateOrDeleteDepartmentCommandHandler(IDepartmentRepository departmentRepository, IMapper mapper) : base(departmentRepository, mapper)
        {
        }

        public async Task<bool> Handle(UpdateOrDeleteDepartmentCommand request, CancellationToken cancellationToken)
        {
            //get the department
            var department = await departmentRepository.GetAsync(u => u.DepartmentID.ToString() == request.DepartmentId && !u.IsDeleted);

            if (department == null)
            {
                throw new NotFoundException("Department", request.DepartmentId);
            }

            // if the name is not present, than update the isDelete flag, else update de name
            if (!string.IsNullOrEmpty(request.Name))
            {
                // check if the name is unique
                if (await departmentRepository.GetAsync(u => u.Name == request.Name && !u.IsDeleted, false) != null)
                {
                    throw new AlreadyExistsException("Department", request.Name);
                }

                department.Name = request.Name;
            }
            else
            {
                if (department.Specializations != null && department.Specializations.Count() > 0)
                {
                    // soft delete if there are specializations under this department
                    department.IsDeleted = true;
                }
                else
                {
                    // hard delete if there is no specialization under this departmentl
                    return await departmentRepository.DeleteAsync(department);
                }
            }

            return await departmentRepository.UpdateAsync(department);
        }
    }
}
