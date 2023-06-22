using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using MediatR;

namespace AttendanceManager.Application.Features.Specialization.Commands.DeleteSpecialization
{
    public sealed class DeleteSpecializationCommand : IRequest<bool>
    {
        public required int SpecializationId { get; init; }
    }

    public sealed class DeleteSpecializationCommandHandler : IRequestHandler<DeleteSpecializationCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteSpecializationCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteSpecializationCommand request, CancellationToken cancellationToken)
        {
            var specialization = await _unitOfWork.SpecializationRepository.GetAsync(s => s.SpecializationID == request.SpecializationId)
                ?? throw new NotFoundException("Specialziation",request.SpecializationId);

            _unitOfWork.SpecializationRepository.Delete(specialization);

            return await _unitOfWork.CommitAsync();
        }
    }
}
