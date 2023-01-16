using AttendanceManager.Application.Contracts.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Department.Commands.DeleteDepartment
{
    public sealed class DeleteDepartmentCommandHandler : BaseFeature, IRequestHandler<DeleteDepartmentCommand, bool>
    {
        public DeleteDepartmentCommandHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public async Task<bool> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
        {
            //get the department
            var department = await unitOfWork.DepartmentRepository.GetAsync(u => u.DepartmentID.ToString() == request.DepartmentID && !u.IsDeleted, Domain.Enums.NavigationPropertiesSetting.OnlyCollectionNavigationProps)
                ?? throw new NotFoundException("Department", request.DepartmentID);

            if (department.Specializations != null && department.Specializations.Count() > 0)
            {
                // soft delete if there are specializations under this department
                department.IsDeleted = true;
                unitOfWork.DepartmentRepository.Update(department);
            }
            else
            {
                // hard delete if there is no specialization under this departmentl
                unitOfWork.DepartmentRepository.Delete(department);
            }

            return await unitOfWork.CommitAsync();
        }
    }
}
