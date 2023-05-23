namespace AttendanceManager.Application.Exceptions
{
    public sealed class NoContentException : ApplicationException
    {
        public NoContentException(string message) : base(message)
        {

        }
    }
}
