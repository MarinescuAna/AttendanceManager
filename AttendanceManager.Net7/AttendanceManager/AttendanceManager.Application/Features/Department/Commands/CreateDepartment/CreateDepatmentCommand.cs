using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Domain.Common;
using MediatR;

namespace AttendanceManager.Application.Features.Department.Commands.CreateDepartment
{
    /// <summary>
    /// Create a new department. This can be done only by the admin
    /// </summary>
    public sealed class CreateDepatmentCommand : IRequest<int>
    {
        public required string Name { get; init; }
    }
    public sealed class CreateDepatmentCommandHandler : IRequestHandler<CreateDepatmentCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateDepatmentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateDepatmentCommand request, CancellationToken cancellationToken)
        {
            // Look for other departments with the same name and throw exception if exists
            if (await _unitOfWork.DepartmentRepository.GetAsync(d => d.Name == request.Name) != null)
            {
                throw new AlreadyExistsException("Department", request.Name);
            }

            // Create the new department
            var newDepartment = new Domain.Entities.Department
            {
                UpdatedOn = DateTime.Now,
                Name = request.Name
            };

            // Add new department or throw exception if something happen
            await _unitOfWork.DepartmentRepository.AddAsync(newDepartment);
            if (!await _unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrongGenericMessage);
            }

            return newDepartment.DepartmentID;
        }
    }
}
