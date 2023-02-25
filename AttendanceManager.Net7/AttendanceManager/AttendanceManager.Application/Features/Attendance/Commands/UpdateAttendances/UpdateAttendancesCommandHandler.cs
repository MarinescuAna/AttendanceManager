using AttendanceManager.Application.Contracts.UnitOfWork;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Attendance.Commands.UpdateAttendances
{
    public sealed class UpdateAttendancesCommandHandler : BaseFeature, IRequestHandler<UpdateAttendancesCommand, bool>
    {
        public UpdateAttendancesCommandHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public async Task<bool> Handle(UpdateAttendancesCommand request, CancellationToken cancellationToken)
        {
            foreach (var student in request.Students)
            {
                var oldStudent = await unitOfWork.AttendanceRepository.GetAsync(a => a.AttendanceID == student.AttendanceID);

                if (oldStudent != null)
                {
                    oldStudent!.IsPresent = student.IsPresent;
                    oldStudent!.UpdatedOn = DateTime.UtcNow;
                    oldStudent!.BonusPoints = student.BonusPoints;

                    unitOfWork.AttendanceRepository.Update(oldStudent);
                }
            }

            return await unitOfWork.CommitAsync();
        }
    }
}
