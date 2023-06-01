using AttendanceManager.Application.Contracts.Infrastructure.Singleton;
using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Application.Features.Reward.Commands.CreateReward;
using AttendanceManager.Domain.Common;
using AttendanceManager.Domain.Enums;
using MediatR;

namespace AttendanceManager.Application.Features.Attendance.Commands.UpdateStudentsInvolvement
{
    public sealed class UpdateStudentsInvolvementCommand : IRequest<bool>
    {
        public required StudentInvolvementVm[] Involvements { get; init; }
    }

    public sealed class UpdateStudentsInvolvementCommandHandler : IRequestHandler<UpdateStudentsInvolvementCommand, bool>
    {
        private IMediator _mediator;
        private IUnitOfWork _unitOfWork;
        private IReportSingleton _currentReport;
        public UpdateStudentsInvolvementCommandHandler(IMediator mediator, IUnitOfWork unit, IReportSingleton reportSingleton)
        {
            _mediator = mediator;
            _unitOfWork = unit;
            _currentReport = reportSingleton;

            if (_currentReport.CurrentReportInfo == null)
            {
                throw new Exceptions.NotImplementedException(ErrorMessages.NoContentReportBaseMessage);
            }
        }

        public async Task<bool> Handle(UpdateStudentsInvolvementCommand request, CancellationToken cancellationToken)
        {
            var attendances = _unitOfWork.AttendanceRepository.GetAttendancesByReportId(_currentReport.CurrentReportInfo.ReportId);
            foreach (var student in request.Involvements)
            {
                var oldStudent = attendances.FirstOrDefault(a => a.AttendanceID == student.InvolvementId);
                if (oldStudent != null)
                {
                    oldStudent.IsPresent = student.IsPresent;
                    oldStudent.UpdatedOn = DateTime.Now;
                    oldStudent.BonusPoints = student.BonusPoints;

                    _unitOfWork.AttendanceRepository.Update(oldStudent);
                }
            }

            if (!await _unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrongInserAttendancesMessage);
            }

            var involvementsWithPresents = request.Involvements.Where(i => i.IsPresent);
            foreach (var involvment in involvementsWithPresents)
            {
                await _mediator.Send(new CreateRewardCommand()
                {
                    RoleRole = Role.Student,
                    UserId = involvment.UserId,
                    CollectionId = involvment.CollectionId,
                    CommitChanges = false
                });
            }

            if (involvementsWithPresents.Count() != 0 && !await _unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrongInsertBadgeMessage);
            }

            return true;
        }
    }
}
