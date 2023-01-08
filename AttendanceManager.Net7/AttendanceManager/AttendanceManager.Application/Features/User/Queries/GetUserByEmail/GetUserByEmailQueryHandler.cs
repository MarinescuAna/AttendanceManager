using AttendanceManager.Application.Contracts.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.User.Queries.GetUserByEmail
{
    public sealed class GetUserByEmailQueryHandler : BaseFeature, IRequestHandler<GetUserByEmailQuery, UserDto>
    {
        public GetUserByEmailQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
        /// <summary>
        ///  Get the user by email
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<UserDto> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
            => mapper.Map<UserDto>(await unitOfWork.UserRepository.GetAsync(u => u.Email == request.Email) 
                ?? throw new NotFoundException(nameof(User), request.Email));

        
    }
}
