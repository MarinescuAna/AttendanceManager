using MediatR;

namespace AttendanceManager.Application.Features.Specialization.Queries.GetSpecializations
{
    public sealed class GetSpecializationsQuery : IRequest<SpecializationDto[]>
    {
    }
}
