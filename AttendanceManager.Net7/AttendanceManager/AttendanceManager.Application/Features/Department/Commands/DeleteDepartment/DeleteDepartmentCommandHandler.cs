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
            // soft delete if there are specializations under this department
            // hard delete if there is no specialization under this departmentl
            if (!await unitOfWork.DepartmentRepository.SoftOrHardDelete(request.DepartmentID))
            {
                throw new NotFoundException("Department", request.DepartmentID);
            }

            return await unitOfWork.CommitAsync();
        }
    }
}
