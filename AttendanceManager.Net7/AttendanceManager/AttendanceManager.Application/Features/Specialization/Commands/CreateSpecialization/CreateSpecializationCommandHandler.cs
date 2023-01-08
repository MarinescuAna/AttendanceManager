using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Application.Contracts.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Application.Shared;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Specialization.Commands.CreateSpecialization
{
    public sealed class CreateSpecializationCommandHandler : BaseFeature, IRequestHandler<CreateSpecializationCommand, SpecializationDto>
    {
        public CreateSpecializationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<SpecializationDto> Handle(CreateSpecializationCommand request, CancellationToken cancellationToken)
        {
            // Look for antoher specializations that have the same name and department and throw exception
            var specialization = await unitOfWork.SpecializationRepository.GetAsync(s => s.Name == request.Name && s.DepartmentID == request.DepartmentId);

            if (specialization != null)
            {
                throw new AlreadyExistsException("Specialization", request.Name);
            }

            // Create the new specialization
            var newSpecialization = new Domain.Entities.Specialization
            {
                DepartmentID = request.DepartmentId,
                Name = request.Name,
                IsDeleted = false,
                SpecializationID = Guid.NewGuid()
            };

            // Save the specialization or throw exception if something happen
            unitOfWork.SpecializationRepository.AddAsync(newSpecialization);
            if (!await unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(Constants.SomethingWentWrongMessage);
            }

            // The specialziation id is mandatory later
            return mapper.Map<SpecializationDto>(newSpecialization);
        }
    }
}
