using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.User.Queries.GetRefreshTokenByEmail
{
    public sealed class GetRefreshTokenByEmailQueryHandler : BaseFeature, IRequestHandler<GetRefreshTokenByEmailQuery, string?>
    {
        public GetRefreshTokenByEmailQueryHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public async Task<string?> Handle(GetRefreshTokenByEmailQuery request, CancellationToken cancellationToken)
            => (await unitOfWork.UserRepository.GetAsync(u => u.Email == request.Email && !u.IsDeleted))?.RefreshToken;
    }
}
