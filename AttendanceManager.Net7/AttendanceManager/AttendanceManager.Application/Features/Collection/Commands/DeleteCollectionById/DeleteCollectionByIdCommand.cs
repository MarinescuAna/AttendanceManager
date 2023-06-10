using AttendanceManager.Application.Contracts.Infrastructure.Singleton;
using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Domain.Common;
using MediatR;

namespace AttendanceManager.Application.Features.Collection.Commands.DeleteCollectionById
{
    public sealed class DeleteCollectionByIdCommand : IRequest<bool>
    {
        public required int CollectionId { get; init; }
    }
    /// <summary>
    /// 1. Delete collection only, all the involvements asociated with it will be deleted
    /// 2. Update order according with the report id and activity type
    /// </summary>
    public sealed class DeleteCollectionByIdCommandHandler : IRequestHandler<DeleteCollectionByIdCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public IReportSingleton _currentReport;
        public DeleteCollectionByIdCommandHandler(IUnitOfWork unitOfWork, IReportSingleton currentReport)
        {
            _unitOfWork = unitOfWork;
            _currentReport = currentReport;

            if (_currentReport.CurrentReportInfo == null)
            {
                throw new Exceptions.NotImplementedException(ErrorMessages.NoContentReportBaseMessage);
            }
        }
        public async Task<bool> Handle(DeleteCollectionByIdCommand request, CancellationToken cancellationToken)
        {
            var currentCollection = await _unitOfWork.AttendanceCollectionRepository.GetAsync(c=>c.AttendanceCollectionID == request.CollectionId)
                ?? throw new NotFoundException("Collection", request.CollectionId);

            _unitOfWork.AttendanceCollectionRepository.Delete(currentCollection);

            if (!await _unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrongGenericMessage);
            }

            //update order
            var collections = _unitOfWork.AttendanceCollectionRepository.ListAll()
                .Where(c => c.CourseType.Equals(currentCollection.CourseType))
                .Where(c => c.DocumentID == _currentReport.CurrentReportInfo.ReportId)
                .Where(c => c.Order > currentCollection.Order).ToList();

            //decrement all the collections and update the database
            if (collections.Count() > 0)
            {
                collections.ForEach(c => c.Order--);
                _unitOfWork.AttendanceCollectionRepository.UpdateRange(collections);

                if (!await _unitOfWork.CommitAsync())
                {
                    throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrongGenericMessage);
                }
            }

            //update currentReport
            _currentReport.LastCollectionOrder[currentCollection.CourseType]--;
            return true;
        }
    }
}
