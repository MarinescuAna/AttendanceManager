using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Domain.Common;
using MediatR;

namespace AttendanceManager.Application.Features.Specialization.Commands.CreateSpecialization
{
    public sealed class CreateSpecializationCommand : IRequest<int>
    {
        public required string Name { get; init; }
        public required int DepartmentId { get; init; }
    }
    public sealed class CreateSpecializationCommandHandler : IRequestHandler<CreateSpecializationCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateSpecializationCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateSpecializationCommand request, CancellationToken cancellationToken)
        {
            // Look for antoher specializations that have the same name and department and throw exception
            var specialization = await _unitOfWork.SpecializationRepository.GetAsync(s => s.Name == request.Name && s.DepartmentID == request.DepartmentId);

            if (specialization != null)
            {
                throw new AlreadyExistsException("Specialization", request.Name);
            }

            // Create the new specialization
            var newSpecialization = new Domain.Entities.Specialization
            {
                DepartmentID = request.DepartmentId,
                Name = request.Name,
                UpdatedOn = DateTime.Now,
            };

            // Save the specialization or throw exception if something happen
            _unitOfWork.SpecializationRepository.AddAsync(newSpecialization);
            if (!await _unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrongGenericMessage);
            }

            return newSpecialization.SpecializationID;
        }
    }
}
