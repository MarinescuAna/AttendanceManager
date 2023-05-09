using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.User.Queries.GetRefreshTokens
{
    public sealed class GetRefreshTokensQueryHandler : BaseFeature, IRequestHandler<GetRefreshTokensQuery, string[]?>
    {
        public GetRefreshTokensQueryHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public async Task<string[]?> Handle(GetRefreshTokensQuery request, CancellationToken cancellationToken)
            => (await unitOfWork.UserRepository.ListAllAsync())
            .Where(u => !string.IsNullOrEmpty(u?.RefreshToken) && !u.IsDeleted && DateTime.Now.CompareTo(u?.ExpRefreshToken) < 0)
            .Select(u => u?.RefreshToken).ToArray();
    }
}
