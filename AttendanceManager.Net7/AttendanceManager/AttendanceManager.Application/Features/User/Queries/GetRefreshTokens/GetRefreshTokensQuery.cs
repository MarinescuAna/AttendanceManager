using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.User.Queries.GetRefreshTokens
{
    public sealed class GetRefreshTokensQuery : IRequest<string[]?>
    {
    }
    public sealed class GetRefreshTokensQueryHandler : BaseFeature, IRequestHandler<GetRefreshTokensQuery, string[]?>
    {
        public GetRefreshTokensQueryHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public Task<string[]?> Handle(GetRefreshTokensQuery request, CancellationToken cancellationToken)
            => Task.FromResult(unitOfWork.UserRepository.ListAll()
                        .Where(u => !string.IsNullOrEmpty(u.RefreshToken))
                        .Where(u => !u.IsDeleted && u.ExpRefreshToken < DateTime.Now)
                        .Select(u => u.RefreshToken).ToArray());
    }
}
