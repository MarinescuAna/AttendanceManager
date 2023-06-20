using AttendanceManager.Application.Contracts.Infrastructure.Singleton;
using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Domain.Common;
using MediatR;

namespace AttendanceManager.Application.Features.Member.Commands.InsertCollaborator
{
    public sealed class InsertCollaboratorCommand : IRequest<MembersVm>
    {
        public required string Email { get; init; }
        public string? CurrentUsername { get; set; }

        public string? CurrentEmail { get; set; }
    }

    public sealed class InsertCollaboratorCommandHandler : IRequestHandler<InsertCollaboratorCommand, MembersVm>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReportSingleton _currentReport;
        public InsertCollaboratorCommandHandler(IUnitOfWork unit, IReportSingleton reportSingleton)
        {
            _unitOfWork = unit;
            _currentReport = reportSingleton;

            if (_currentReport.CurrentReportInfo == null)
            {
                throw new Exceptions.NotImplementedException(ErrorMessages.NoContentReportBaseMessage);
            }
        }

        public async Task<MembersVm> Handle(InsertCollaboratorCommand request, CancellationToken cancellationToken)
        {
            if (request.Email.Equals(request.CurrentEmail))
            {
                throw new BadRequestException(ErrorMessages.BadRequest_InvalidParameters_Collaborator_Error);
            }

            //check if the user exists and if the user is teacher
            var user = await _unitOfWork.UserRepository.GetAsync(u => u.Email == request.Email)
                ?? throw new NotFoundException("Teacher", request.Email);

            if (user.Role != Domain.Enums.Role.Teacher)
            {
                throw new BadRequestException(string.Format(ErrorMessages.BadRequest_CollaboratorInsert_Error, request.Email));
            }

            //check if the user is already memeber
            if (await _unitOfWork.MemberRepository.GetAsync(m => m.UserID.Equals(request.Email) && m.DocumentID == _currentReport.CurrentReportInfo.ReportId) != null)
            {
                throw new AlreadyExistsException(ErrorMessages.AlreadyExists_CollaboratorInsert_Error);
            }

            // add the new teacher as collaborator
            _unitOfWork.MemberRepository.AddAsync(new Domain.Entities.Member
            {
                MemberID = Guid.NewGuid(),
                DocumentID = _currentReport.CurrentReportInfo.ReportId,
                UserID = request.Email
            });

            //send notification
            _unitOfWork.NotificationRepository.AddAsync(new()
            {
                CreatedOn = DateTime.Now,
                IsRead = false,
                Message = string.Format(NotificationMessages.CollaboratorAddedNotification, request.CurrentUsername, _currentReport.CurrentReportInfo.Title),
                Priority = Domain.Enums.NotificationPriority.Info,
                UserID = request.Email
            });

            if (await _unitOfWork.CommitAsync())
            {
                return new()
                {
                    Email = request.Email,
                    Name = user.FullName
                };
            }

            throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrongGenericMessage);
        }
    }
}
