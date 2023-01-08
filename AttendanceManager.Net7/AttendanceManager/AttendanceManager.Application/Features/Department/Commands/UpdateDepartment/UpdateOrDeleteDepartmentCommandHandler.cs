using AttendanceManager.Application.Contracts.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Department.Commands.UpdateDepartment
{
    public sealed class UpdateOrDeleteDepartmentCommandHandler : BaseFeature, IRequestHandler<UpdateOrDeleteDepartmentCommand, bool>
    {
        public UpdateOrDeleteDepartmentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<bool> Handle(UpdateOrDeleteDepartmentCommand request, CancellationToken cancellationToken)
        {
            //get the department
            var department = await unitOfWork.DepartmentRepository.GetAsync(u => u.DepartmentID.ToString() == request.DepartmentId && !u.IsDeleted, Domain.Enums.NavigationPropertiesSetting.OnlyCollectionNavigationProps)
                ?? throw new NotFoundException("Department", request.DepartmentId);

            // if the name is not present, than update the isDelete flag, else update de name
            if (!string.IsNullOrEmpty(request.Name))
            {
                // check if the name is unique
                if (await unitOfWork.DepartmentRepository.GetAsync(u => u.Name == request.Name && !u.IsDeleted) != null)
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
                    unitOfWork.DepartmentRepository.Delete(department);
                }
            }

            unitOfWork.DepartmentRepository.Update(department);
            return await unitOfWork.CommitAsync();
        }
    }
}
