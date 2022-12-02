using System;

namespace AttendanceManager.Application.Exceptions
{
    public class SomethingWentWrongException : ApplicationException
    {
        public SomethingWentWrongException(string message) : base(message)
        {

        }
    }
}
