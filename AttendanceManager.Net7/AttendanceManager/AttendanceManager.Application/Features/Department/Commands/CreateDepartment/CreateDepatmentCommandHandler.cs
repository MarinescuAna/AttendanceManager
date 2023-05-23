using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Domain.Common;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Department.Commands.CreateDepartment
{
    public sealed class CreateDepatmentCommandHandler : BaseFeature, IRequestHandler<CreateDepatmentCommand, int>
    {
        public CreateDepatmentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<int> Handle(CreateDepatmentCommand request, CancellationToken cancellationToken)
        {
            // Look for other departments with the same name and throw exception if exists
            if (await unitOfWork.DepartmentRepository.GetAsync(d => d.Name == request.Name) != null)
            {
                throw new AlreadyExistsException("Department", request.Name);
            }

            // Create the new department
            var newDepartment = new Domain.Entities.Department
            {
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now,
                Name = request.Name
            };

            // Add new department or throw exception if something happen
            unitOfWork.DepartmentRepository.AddAsync(newDepartment);
            if (!await unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrongGenericMessage);
            }

            return newDepartment.DepartmentID;
        }
    }
}
