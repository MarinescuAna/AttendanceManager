using AttendanceManager.Application.Contracts.Infrastructure.Singleton;
using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Application.Features.Notification.Commands.CreateNotification;
using AttendanceManager.Application.Features.Reward.Commands.CreateReward;
using AttendanceManager.Domain.Common;
using MediatR;

namespace AttendanceManager.Application.Features.InvolvementCode.Commands.CreateInvolvementCode
{
    public sealed class CreateInvolvementCodeCommand : IRequest<InvolvementCodeVm>
    {
        public required int Minutes { get; init; }
        public required int CollectionId { get; init; }
        public string? UserId { get; set; }
    }

    public sealed class CreateInvolvementCodeCommandHandler : IRequestHandler<CreateInvolvementCodeCommand, InvolvementCodeVm>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly IReportSingleton _currentReport;
        public CreateInvolvementCodeCommandHandler(IUnitOfWork unit, IMediator mediator, IReportSingleton currentReport)
        {
            _unitOfWork = unit;
            _mediator = mediator;
            _currentReport= currentReport;

            if (_currentReport.CurrentReportInfo == null)
            {
                throw new Exceptions.NotImplementedException(ErrorMessages.NoContentReportBaseMessage);
            }
        }

        public async Task<InvolvementCodeVm> Handle(CreateInvolvementCodeCommand request, CancellationToken cancellationToken)
        {
            var codes = _unitOfWork.InvolvementCodeRepository.ListAll();
            var code = string.Empty;

            // generate code
            do
            {
                code = new string(Enumerable.Repeat(Constants.CharsString, Constants.AttendanceCodeLength).Select(s => s[new Random().Next(s.Length)]).ToArray());
            } while (codes.Any(c => c.Code.Equals(code)));

            // save the code
            var newCode = new Domain.Entities.InvolvementCode()
            {
                Code = code,
                ExpirationDate = DateTime.Now.AddMinutes(request.Minutes),
                AttendanceCollectionId = request.CollectionId
            };
            _unitOfWork.InvolvementCodeRepository.AddAsync(newCode);

            foreach (var student in _currentReport.Members.Where(m => m.Value == Domain.Enums.Role.Student))
            {
                await _mediator.Send(new CreateNotificationCommand()
                {
                    CommitChanges = false,
                    Message = string.Format(NotificationMessages.CreateCodeNotification, request.UserId, _currentReport.CurrentReportInfo.Title,request.Minutes),
                    Priority = Domain.Enums.NotificationPriority.Warning,
                    UserEmail = student.Key
                });
            }

            if (!await _unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrongGenericMessage);
            }

            await _mediator.Send(new CreateRewardCommand()
            {
                CurrentCollectionId = request.CollectionId,
                AchievedUserRole = Domain.Enums.Role.Teacher,
                AchievedUserId = request.UserId!,
                CommitChanges = true
            });

            return new()
            {
                Code = newCode.Code,
                ExpirationDate = newCode.ExpirationDate.ToShortTimeString()
            };
        }
    }
}
