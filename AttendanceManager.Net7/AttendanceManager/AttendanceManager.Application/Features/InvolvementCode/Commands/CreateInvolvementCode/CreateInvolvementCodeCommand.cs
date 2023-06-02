using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Application.Features.Reward.Commands.CreateReward;
using AttendanceManager.Domain.Common;
using MediatR;

namespace AttendanceManager.Application.Features.InvolvementCode.Commands.CreateInvolvementCode
{
    public sealed class CreateInvolvementCodeCommand : IRequest<InvolvementCodeVm>
    {
        public required int Minutes { get; init; }
        public required int CollectionId { get; init; }
        public required string UserId { get; init; }
    }

    public sealed class CreateInvolvementCodeCommandHandler : IRequestHandler<CreateInvolvementCodeCommand, InvolvementCodeVm>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        public CreateInvolvementCodeCommandHandler(IUnitOfWork unit, IMediator mediator)
        {
            _unitOfWork = unit;
            _mediator = mediator;
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

            if (!await _unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrongGenericMessage);
            }

            await _mediator.Send(new CreateRewardCommand()
            {
                CurrentCollectionId = request.CollectionId,
                AchievedUserRole = Domain.Enums.Role.Teacher,
                AchievedUserId = request.UserId,
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
