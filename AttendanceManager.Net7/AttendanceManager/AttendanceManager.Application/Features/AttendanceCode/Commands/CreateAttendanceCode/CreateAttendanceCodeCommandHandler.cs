using AttendanceManager.Application.Contracts.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Application.Shared;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.AttendanceCode.Commands.CreateAttendanceCode
{
    public sealed class CreateAttendanceCodeCommandHandler : BaseFeature, IRequestHandler<CreateAttendanceCodeCommand, AttendanceCodeDTO>
    {
        public CreateAttendanceCodeCommandHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public async Task<AttendanceCodeDTO> Handle(CreateAttendanceCodeCommand request, CancellationToken cancellationToken)
        {

            var codes = await unitOfWork.AttendanceCodeRepository.ListAllAsync();
            var code = string.Empty;

            // generate code
            do
            {
                code = new string(Enumerable.Repeat(Constants.CharsString, Constants.AttendanceCodeLength).Select(s => s[new Random().Next(s.Length)]).ToArray());
            } while (codes.Any(c => c.Code.Equals(code)));

            // save the code
            var newCode = new Domain.Entities.AttendanceCode()
            {
                Code = code,
                ExpirationDate = DateTime.Now.AddMinutes(request.Minutes),
                AttendanceCollectionId= request.AttendanceCollectionId
            };
            unitOfWork.AttendanceCodeRepository.AddAsync(newCode);

            if(!await unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(Constants.SomethingWentWrongMessage);
            }

            return new() {
                Code = newCode.Code,
                ExpirationDate = newCode.ExpirationDate.ToShortTimeString()
            };
        }
    }
}
