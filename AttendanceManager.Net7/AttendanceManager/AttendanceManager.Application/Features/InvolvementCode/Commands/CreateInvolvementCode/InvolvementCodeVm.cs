namespace AttendanceManager.Application.Features.InvolvementCode.Commands.CreateInvolvementCode
{
    public sealed class InvolvementCodeVm
    {
        public required string Code { get; init; }
        public required string ExpirationDate { get; init; }

    }
}
