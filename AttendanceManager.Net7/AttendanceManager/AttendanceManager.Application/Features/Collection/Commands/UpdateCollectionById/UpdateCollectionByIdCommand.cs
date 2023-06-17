using AttendanceManager.Application.Contracts.Infrastructure.Singleton;
using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Domain.Common;
using AttendanceManager.Domain.Enums;
using MediatR;
using System.Globalization;

namespace AttendanceManager.Application.Features.Collection.Commands.UpdateCollectionById
{
    public sealed class UpdateCollectionByIdCommand : IRequest<bool>
    {
        public required int CollectionId { get; init; }
        public required string ActivityDateTime { get; init; }
        public required string CourseType { get; init; }
        public string? Title { get; init; }
    }

    public sealed class UpdateCollectionByIdCommandHandler : IRequestHandler<UpdateCollectionByIdCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public IReportSingleton _currentReport;
        public UpdateCollectionByIdCommandHandler(IUnitOfWork unitOfWork, IReportSingleton currentReport)
        {
            _unitOfWork = unitOfWork;
            _currentReport = currentReport;

            if (_currentReport.CurrentReportInfo == null)
            {
                throw new Exceptions.NotImplementedException(ErrorMessages.NoContentReportBaseMessage);
            }
        }
        public async Task<bool> Handle(UpdateCollectionByIdCommand request, CancellationToken cancellationToken)
        {
            var oldCollection = await _unitOfWork.CollectionRepository.GetAsync(s => s.CollectionID == request.CollectionId) ??
                throw new NotFoundException("Collection", request.CollectionId);

            //TODO check date, type and username
            if (!DateTime.TryParseExact(request.ActivityDateTime, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedActivityTime))
            {
                throw new BadRequestException(ErrorMessages.BadRequest_CreateCollectionParams2_Error);
            }

            if (!Enum.TryParse(request.CourseType, out CourseType courseType))
            {
                throw new BadRequestException(ErrorMessages.BadRequest_CreateCollectionParams3_Error);
            }

            oldCollection.CourseType = courseType;
            oldCollection.HeldOn = parsedActivityTime;
            oldCollection.Title = request.Title;

            if (!await _unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrongGenericMessage);
            }

            if (!oldCollection.CourseType.Equals(courseType))
            {
                _currentReport.ReportCollectionTypes[request.CollectionId] = courseType;
            }
            return true;
        }
    }
}
