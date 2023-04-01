using AttendanceManager.Application.Contracts.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Application.Features.Dashboard.Helpers;
using AttendanceManager.Domain.Enums;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Dashboard.Queries.GetDashboardForDocumentById
{
    public sealed class GetDashboardForDocumentByIdQueryHandler : BaseDocumentFeature, IRequestHandler<GetDashboardForDocumentByIdQuery, DocumentDashboardDto>
    {
        public GetDashboardForDocumentByIdQueryHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public Task<DocumentDashboardDto> Handle(GetDashboardForDocumentByIdQuery request, CancellationToken cancellationToken)
        {
            if(attendanceCollectionsType == null)
            {
                throw new BadRequestException("There is no attendance collection defined!");
            }

            var finalResult = new DocumentDashboardDto();
            var studentMembers = documentMembers!.Where(m => m.User!.Role == Role.Student);

            // compute the student interest
            var computeStudentInterestHelper = new ComputeStudentInterestPerDocumentFactory(attendanceCollectionsType,currentDocument!,studentMembers);
            finalResult.StudentInterests = computeStudentInterestHelper.Compute();

            return Task.FromResult(finalResult);
        }
       
    }
}
