namespace AttendanceManager.Application.Exceptions
{
    public sealed class AlreadyExistsException : ApplicationException
    {
        public AlreadyExistsException(string name, string value)
            : base($"{name} with {value} already exists!")
        {
        }
        public AlreadyExistsException(string name)
            : base(name)
        {
        }
    }
}
