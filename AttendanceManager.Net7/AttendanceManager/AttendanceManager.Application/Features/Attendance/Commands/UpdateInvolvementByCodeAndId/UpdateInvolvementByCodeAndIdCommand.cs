using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Domain.Enums;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Attendance.Commands.UpdateInvolvementByCodeAndId
{
    public sealed class UpdateInvolvementByCodeAndIdCommand : IRequest<bool>
    {
        public required string Code { get; init; }
        public required int AttendanceId { get; init; }
        public required int AttendanceCollectionId { get; init; }
    }

    public sealed class UpdateInvolvementByCodeAndIdCommandHandler : IRequestHandler<UpdateInvolvementByCodeAndIdCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateInvolvementByCodeAndIdCommandHandler(IUnitOfWork unit)
        {
            _unitOfWork = unit;
        }

        public async Task<bool> Handle(UpdateInvolvementByCodeAndIdCommand request, CancellationToken cancellationToken)
        {
            // check if the code exists into the database
            var code = await _unitOfWork.InvolvementCodeRepository.GetAsync(c => c.Code.Equals(request.Code) && c.AttendanceCollectionId == request.AttendanceCollectionId)
                ?? throw new NotFoundException("Code", request.Code);

            // check if the code is still valid
            if (code.ExpirationDate.CompareTo(DateTime.Now) < 0)
            {
                throw new SomethingWentWrongException("The code has expired!");
            }

            // get the attendance
            var attendance = await _unitOfWork.AttendanceRepository.GetAsync(a => a.AttendanceID.Equals(request.AttendanceId))
                ?? throw new NotFoundException("Attendance", request.AttendanceId);

            //update the attendance
            attendance.IsPresent = true;
            _unitOfWork.AttendanceRepository.Update(attendance);

            return await _unitOfWork.CommitAsync();
        }

    }
}
