using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Domain.Common;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.InvolvementCode.Commands.CreateInvolvementCode
{
    public sealed class CreateInvolvementCodeCommand : IRequest<InvolvementCodeDto>
    {
        public required int Minutes { get; init; }
        public required int AttendanceCollectionId { get; init;}
    }

    public sealed class CreateInvolvementCodeCommandHandler : BaseFeature, IRequestHandler<CreateInvolvementCodeCommand, InvolvementCodeDto>
    {
        public CreateInvolvementCodeCommandHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public async Task<InvolvementCodeDto> Handle(CreateInvolvementCodeCommand request, CancellationToken cancellationToken)
        {

            var codes = await unitOfWork.InvolvementCodeRepository.ListAllAsync();
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
                AttendanceCollectionId = request.AttendanceCollectionId
            };
            unitOfWork.InvolvementCodeRepository.AddAsync(newCode);

            if (!await unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrongGenericMessage);
            }

            return new()
            {
                Code = newCode.Code,
                ExpirationDate = newCode.ExpirationDate.ToShortTimeString()
            };
        }
    }
}
