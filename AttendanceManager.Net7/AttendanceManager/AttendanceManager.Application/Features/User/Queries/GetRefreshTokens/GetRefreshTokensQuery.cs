using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.User.Queries.GetRefreshTokens
{
    public sealed class GetRefreshTokensQuery : IRequest<string[]?>
    {
    }
    public sealed class GetRefreshTokensQueryHandler : IRequestHandler<GetRefreshTokensQuery, string[]?>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetRefreshTokensQueryHandler(IUnitOfWork unit)
        {
            _unitOfWork = unit;
        }

        public Task<string[]?> Handle(GetRefreshTokensQuery request, CancellationToken cancellationToken)
            => Task.FromResult(_unitOfWork.UserRepository.ListAll()
                        .Where(u => !string.IsNullOrEmpty(u.RefreshToken))
                        .Where(u => !u.IsDeleted && u.ExpRefreshToken < DateTime.Now)
                        .Select(u => u.RefreshToken).ToArray());
    }
}
