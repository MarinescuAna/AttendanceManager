using System;

namespace AttendanceManager.Application.Exceptions
{
    public class AlreadyExistsException : ApplicationException
    {
        public AlreadyExistsException(string name, string value)
            : base($"{name} with {value} already exists!")
        {
        }
    }
}
