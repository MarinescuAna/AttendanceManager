using AttendanceManager.Application.Contracts.Infrastructure.Mail;
using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Application.Models.Mail;
using AttendanceManager.Domain.Common;
using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;
using MediatR;

namespace AttendanceManager.Application.Features.User.Commands.CreateMultipleUsers
{
    public sealed class CreateMultipleUsersCommand:IRequest<int>
    {
        public required UserVm[] Users { get; init; }
    }
    public sealed class CreateMultipleUsersCommandHandler : IRequestHandler<CreateMultipleUsersCommand, int>
    {
        private readonly IMailService _mailService;
        private readonly IUnitOfWork _unitOfWork;

        public CreateMultipleUsersCommandHandler(IUnitOfWork unitOfWork, IMailService mailService)
        {
            _unitOfWork = unitOfWork;
            _mailService = mailService;
        }

        public async Task<int> Handle(CreateMultipleUsersCommand request, CancellationToken cancellationToken)
        {
            var users = new List<Domain.Entities.User>();
            foreach (var user in request.Users)
            {
                if (await _unitOfWork.UserRepository.GetAsync(u => u.Email.Equals(user.Email)) != null)
                {
                    throw new AlreadyExistsException("User", user.Email);
                }

                users.Add(new Domain.Entities.User() {
                    Email = user.Email,
                    Year = user.Year,
                    FullName = user.Fullname,
                    Code = user.Code,
                    Role = Enum.Parse<Role>(user.Role),
                    Password = PasswordGenerator.GeneratePassword(),
                    AccountConfirmed = false,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    UserSpecializations = user.SpecializationIds.Select(id => new UserSpecialization()
                    {
                        SpecializationID = id,
                        UserID = user.Email
                    }).ToList()
                });
            }

            await _unitOfWork.UserRepository.AddRangeAsync(users);

            if (!await _unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrongGenericMessage);
            }
            var counter = 0;
            foreach (var user in users)
            {
                var message = new Message(user.Email, Constants.Subject, string.Format(Constants.Body, user.FullName, user.Email, user.Password), user.FullName);
                if (!await _mailService.SendEmailAsync(message, new CancellationToken()))
                {
                    counter++;
                }
            }

            return counter;
        }
    }
}
