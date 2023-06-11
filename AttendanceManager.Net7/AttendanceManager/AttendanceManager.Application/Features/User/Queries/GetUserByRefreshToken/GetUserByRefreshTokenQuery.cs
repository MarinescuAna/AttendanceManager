using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.User.Queries.GetUserByRefreshToken
{
    public sealed class GetUserByRefreshTokenQuery : IRequest<UserByRefreshTokenVm>
    {
        public required string RefreshToken { get; init; }
    }
    public sealed class GetUserByRefreshTokenQueryHandler : IRequestHandler<GetUserByRefreshTokenQuery, UserByRefreshTokenVm>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetUserByRefreshTokenQueryHandler(IUnitOfWork unit, IMapper mapper)
        {
            _unitOfWork = unit;
            _mapper = mapper;
        }
        /// <summary>
        ///  Get the user by refresh token
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<UserByRefreshTokenVm> Handle(GetUserByRefreshTokenQuery request, CancellationToken cancellationToken)
            => _mapper.Map<UserByRefreshTokenVm>(await _unitOfWork.UserRepository.GetAsync(u => u.RefreshToken == request.RefreshToken && !u.IsDeleted)
                ?? throw new NotFoundException(nameof(User), request.RefreshToken));
    }
}
