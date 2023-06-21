using AttendanceManager.Application.Contracts.Infrastructure.Singleton;
using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Application.Features.Reward.Commands.CreateReward;
using AttendanceManager.Domain.Common;
using AttendanceManager.Domain.Enums;
using MediatR;

namespace AttendanceManager.Application.Features.Involvement.Commands.UpdateStudentsInvolvement
{
    public sealed class UpdateStudentsInvolvementCommand : IRequest<bool>
    {
        public required InvolvementVm[] Involvements { get; init; }
        public string? CurrentUserEmail { get; set; }
        public string? CurrentUserName { get; set; }
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
            var involvements = _unitOfWork.InvolvementRepository.GetInvolvementsByReportId(_currentReport.CurrentReportInfo.ReportId);
            foreach (var student in request.Involvements)
            {
                var oldStudent = involvements.FirstOrDefault(a => a.InvolvementID == student.InvolvementId);
                if (oldStudent != null)
                {
                    oldStudent.IsPresent = student.IsPresent;
                    oldStudent.UpdatedOn = DateTime.Now;
                    oldStudent.BonusPoints = student.BonusPoints;
                    oldStudent.UpdateBy = request.CurrentUserName;

                    _unitOfWork.InvolvementRepository.Update(oldStudent);
                }
            }

            if (!await _unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrongInserAttendancesMessage);
            }

            // check if you can add any badges for students
            var involvementsWithPresents = request.Involvements.Where(i => i.IsPresent);

            foreach (var involvment in involvementsWithPresents)
            {
                await _mediator.Send(new CreateRewardCommand()
                {
                    AchievedUserRole = Role.Student,
                    AchievedUserId = involvment.UserId,
                    CurrentCollectionId = involvment.CollectionId
                });
            }

            var saveChanges = false;
            foreach (var collection in involvementsWithPresents.DistinctBy(c => c.CollectionId).Select(c => c.CollectionId))
            {
                //check if you can add any badges for current teacher
                if (await _mediator.Send(new CreateRewardCommand()
                {
                    AchievedUserRole = Role.Teacher,
                    AchievedUserId = request!.CurrentUserEmail!,
                    CurrentCollectionId = collection,
                    CommitChanges = false
                }))
                {
                    saveChanges = true;
                }
            }

            if (saveChanges && !await _unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrongInsertBadgeMessage);
            }

            return true;
        }
    }
}
