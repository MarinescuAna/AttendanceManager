namespace AttendanceManager.Application.Exceptions
{
    public sealed class NotFoundException : ApplicationException
    {
        public NotFoundException(string name, object key)
            : base($"{name} ({key}) is not found")
        {
        }
    }
}
