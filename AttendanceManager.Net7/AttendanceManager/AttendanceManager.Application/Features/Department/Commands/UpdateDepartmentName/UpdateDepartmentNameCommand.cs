using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using MediatR;

namespace AttendanceManager.Application.Features.Department.Commands.UpdateDepartmentName
{
    /// <summary>
    /// Update only the name
    /// </summary>
    public sealed class UpdateDepartmentNameCommand : IRequest<bool>
    {
        public required int DepartmentId { get; init; }
        public required string Name { get; init; }

    }

    public sealed class UpdateDepartmentNameCommandHandler : IRequestHandler<UpdateDepartmentNameCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateDepartmentNameCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateDepartmentNameCommand request, CancellationToken cancellationToken)
        {
            //get the department
            var department = await _unitOfWork.DepartmentRepository.GetAsync(u => u.DepartmentID == request.DepartmentId)
                ?? throw new NotFoundException("Department", request.DepartmentId);

            // check if the name is unique
            if (await _unitOfWork.DepartmentRepository.GetAsync(u => u.Name == request.Name) != null)
            {
                throw new AlreadyExistsException("Department", request.Name);
            }

            department.Name = request.Name;
            department.UpdatedOn = DateTime.Now;
            _unitOfWork.DepartmentRepository.Update(department);

            return await _unitOfWork.CommitAsync();
        }
    }
}
