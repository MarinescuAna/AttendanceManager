using AttendanceManager.Application.Contracts.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Attendance.Commands.UpdateAttendanceByCodeAndAttendanceId
{
    public sealed class UpdateAttendanceByCodeAndAttendanceIdCommandHandler : BaseFeature, IRequestHandler<UpdateAttendanceByCodeAndAttendanceIdCommand, bool>
    {
        public UpdateAttendanceByCodeAndAttendanceIdCommandHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public async Task<bool> Handle(UpdateAttendanceByCodeAndAttendanceIdCommand request, CancellationToken cancellationToken)
        {
            // check if the code exists into the database
            var code = await unitOfWork.AttendanceCodeRepository.GetAsync(c => c.Code.Equals(request.AttendanceCode) && c.AttendanceCollectionId==request.AttendanceCollectionId)
                ?? throw new NotFoundException("Code", request.AttendanceCode);

            // check if the code is still valid
            if (code.ExpirationDate.CompareTo(DateTime.Now) < 0)
            {
                throw new SomethingWentWrongException("The code has expired!");
            }

            // get the attendance
            var attendance = await unitOfWork.AttendanceRepository.GetAsync(a => a.AttendanceID.Equals(request.AttendanceId))
                ?? throw new NotFoundException("Attendance", request.AttendanceId);

            //update the attendance
            attendance.IsPresent = true;
            unitOfWork.AttendanceRepository.Update(attendance);

            return await unitOfWork.CommitAsync();
        }
    }
}
