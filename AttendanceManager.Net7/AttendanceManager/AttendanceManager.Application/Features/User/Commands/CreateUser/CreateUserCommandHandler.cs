﻿using AttendanceManager.Application.Contracts.Infrastructure.Mail;
using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Application.Models.Mail;
using AttendanceManager.Domain.Common;
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
            if (await unitOfWork.UserRepository.GetAsync(u => u.Email == request.Email && !u.IsDeleted) != null)
            {
                throw new AlreadyExistsException("User", request.Email);
            }

            //create a new user and add the specializations where is part of
            var newUser = new Domain.Entities.User
            {
                Email = request.Email,
                EnrollmentYear = request.Year,
                FullName = request.Fullname,
                Code = request.Code,
                Role = Enum.Parse<Role>(request.Role),
                Password = GeneratePassword(),
                AccountConfirmed = false,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now,
                UserSpecializations = request.SpecializationIds.Select(id => new UserSpecialization()
                {
                    SpecializationID = id,
                    UserID = request.Email
                }).ToList()
            };

            //save the user into the db or throw exception if something happen
            unitOfWork.UserRepository.AddAsync(newUser);
            if (!await unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrongGenericMessage);
            }

            //send email
            var message = new Message(newUser.Email, Constants.Subject, string.Format(Constants.Body, newUser.FullName, newUser.Email, newUser.Password), newUser.FullName);
            if (!await _mailService.SendEmail(message, new CancellationToken()))
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrongGenericMessage);
            }

            return Unit.Value;
        }
        private static string GeneratePassword() => new (Enumerable.Repeat(Constants.CharsString, Constants.PasswordLength).Select(s => s[new Random().Next(s.Length)]).ToArray());

    }
}
