using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using MediatR;

namespace AttendanceManager.Application.Features.Department.Commands.DeleteDepartment
{
    public sealed class DeleteDepartmentCommand : IRequest<bool>
    {
        public required int DepartmentId { get; init; }
    }
    public sealed class DeleteDepartmentCommandHandler : IRequestHandler<DeleteDepartmentCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteDepartmentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = await _unitOfWork.DepartmentRepository.GetAsync(s => s.DepartmentID == request.DepartmentId)
                ?? throw new NotFoundException("Department", request.DepartmentId);

            _unitOfWork.DepartmentRepository.Delete(department);

            return await _unitOfWork.CommitAsync();
        }
    }
}
