using System;

namespace AttendanceManager.Application.Exceptions
{
    /*
     * This class is used if the input is, for example, null when we are expecting to update an event.
     */
    public class BadRequestException : ApplicationException
    {
        public BadRequestException(string message) : base(message)
        {

        }
    }
}
