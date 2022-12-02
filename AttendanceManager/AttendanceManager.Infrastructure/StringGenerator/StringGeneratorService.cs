using AttendanceManager.Infrastructure.Shared;
using MediatR;
using System;
using System.Linq;

namespace AttendanceManager.Infrastructure.StringGenerator
{
    public class StringGeneratorService : Application.Contracts.StringGenerator.IStringGeneratorService
    {
        public string GeneratePassword() => Generate(Constants.PasswordLength);

        private string Generate(int length) =>
            new string(Enumerable.Repeat(Constants.CharsString, length).Select(s => s[new Random().Next(s.Length)]).ToArray());
    }
}
