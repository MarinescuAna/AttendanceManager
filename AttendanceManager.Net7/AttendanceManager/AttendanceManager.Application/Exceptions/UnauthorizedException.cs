namespace AttendanceManager.Application.Exceptions
{
    public sealed class UnauthorizedException : ApplicationException
    {
        public UnauthorizedException(string name)
            : base(name)
        {
        }
    }
}
