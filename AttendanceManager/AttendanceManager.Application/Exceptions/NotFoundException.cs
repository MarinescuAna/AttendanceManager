using System;

namespace AttendanceManager.Application.Exceptions
{
    /*
     * This class will be used if someone wants to update, for example, an event that doesn't exist. 
     */
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string name, object key)
            : base($"{name} ({key}) is not found")
        {
        }
    }
}
