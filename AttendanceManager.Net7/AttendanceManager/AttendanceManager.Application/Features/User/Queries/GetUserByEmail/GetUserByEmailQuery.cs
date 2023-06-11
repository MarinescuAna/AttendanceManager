using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.User.Queries.GetUserByEmail
{
    public sealed class GetUserByEmailQuery : IRequest<UserByEmailVm>
    {
        public required string Email { get; init; }
    }
    public sealed class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, UserByEmailVm>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetUserByEmailQueryHandler(IUnitOfWork unit, IMapper mapper)
        {
            _unitOfWork = unit;
            _mapper = mapper;
        }
        /// <summary>
        ///  Get the user by email
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<UserByEmailVm> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
            => _mapper.Map<UserByEmailVm>(await _unitOfWork.UserRepository.GetAsync(u => u.Email == request.Email && !u.IsDeleted)
                ?? throw new NotFoundException(nameof(User), request.Email));

    }
}
