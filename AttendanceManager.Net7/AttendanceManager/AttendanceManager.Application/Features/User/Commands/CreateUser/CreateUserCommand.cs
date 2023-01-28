﻿using MediatR;

namespace AttendanceManager.Application.Features.User.Commands.CreateUser
{
    public sealed class CreateUserCommand : IRequest
    {
        public required string Email { get; init; }
        public required string Role { get; init; }
        public required string Fullname { get; init; }
        public required int Year { get; init; }
        public required string Code { get; init; }
        public required int[] SpecializationIds { get; init; }
    }
}
