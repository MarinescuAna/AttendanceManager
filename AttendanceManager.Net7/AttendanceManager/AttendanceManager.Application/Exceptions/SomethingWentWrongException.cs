namespace AttendanceManager.Application.Exceptions
{
    public sealed class SomethingWentWrongException : ApplicationException
    {
        public SomethingWentWrongException(string message) : base(message)
        {

        }
    }
}
