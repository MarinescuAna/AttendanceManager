using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Application.Features.User.Queries.Dtos;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.User.Queries.GetUserByRefreshToken
{
    public sealed class GetUserByRefreshTokenQueryHandler : BaseFeature, IRequestHandler<GetUserByRefreshTokenQuery, UserByRefreshTokenDto>
    {
        public GetUserByRefreshTokenQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
        /// <summary>
        ///  Get the user by refresh token
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<UserByRefreshTokenDto> Handle(GetUserByRefreshTokenQuery request, CancellationToken cancellationToken)
            => mapper.Map<UserByRefreshTokenDto>(await unitOfWork.UserRepository.GetAsync(u => u.RefreshToken == request.RefreshToken && !u.IsDeleted) 
                ?? throw new NotFoundException(nameof(User), request.RefreshToken));

        
    }
}
