using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceManager.Application.Exceptions
{
    public class AlreadyExistsException:ApplicationException
    {
        public AlreadyExistsException(string name, string value)
            : base($"{name} with {value} already exists!")
        {
        }
    }
}
