using AttendanceManager.Application.Contracts.Mail;
using AttendanceManager.Application.Contracts.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Application.Models.Mail;
using AttendanceManager.Application.Shared;
using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.User.Commands.CreateUser
{
    public sealed class CreateUserCommandHandler : BaseFeature, IRequestHandler<CreateUserCommand>
    {
        private readonly IMailService _mailService;
        public CreateUserCommandHandler(IUnitOfWork unit, IMapper map, IMailService mailService) : base(unit, map)
        {
            _mailService = mailService;
        }

        public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // Check if the user dosen't already have an account
            if (await unitOfWork.UserRepository.GetAsync(u => u.Email == request.Email) != null)
            {
                throw new AlreadyExistsException("User", request.Email);
            }

            //create a new user and add the specializations where is part of
            var userId = Guid.NewGuid();
            var newUser = new Domain.Entities.User
            {
                Email = request.Email,
                EnrollmentYear = int.Parse(request.Year),
                FullName = request.Fullname,
                Code = request.Code,
                Role = Enum.Parse<Role>(request.Role),
                UserID = userId,
                Password = GeneratePassword(),
                AccountConfirmed = false,
                Created = DateTime.Now,
                Updated = DateTime.Now,
                UserSpecializations = request.Specializations.Select(s => new UserSpecialization()
                {
                    SpecializationID = Guid.Parse(s),
                    UserID = userId,
                    UserSpecializationID = Guid.NewGuid()
                }).ToList()
            };

            //save the user into the db or throw exception if something happen
            unitOfWork.UserRepository.AddAsync(newUser);
            if (!await unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(Constants.SomethingWentWrongMessage);
            }

            //send email
            var message = new Message(newUser.Email, Constants.Subject, String.Format(Constants.Body, newUser.FullName, newUser.Email, newUser.Password), newUser.FullName);
            if (!await _mailService.SendEmail(message, new CancellationToken()))
            {
                throw new SomethingWentWrongException(Constants.SomethingWentWrongMessageEmailSend);
            }

            return Unit.Value;
        }
        private static string GeneratePassword() => new (Enumerable.Repeat(Constants.CharsString, Constants.PasswordLength).Select(s => s[new Random().Next(s.Length)]).ToArray());

    }
}
